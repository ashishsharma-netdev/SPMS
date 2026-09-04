using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SPMS.Data;
using SPMS.Services;
using System.Text;
using StackExchange.Redis;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ThisIsADevSecretKey_ForDemoPurposesOnly!ChangeInProd";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SPMS";

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header, Description = "Enter JWT token as: Bearer {token}" });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } });
});

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(conn)) builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn));
else builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("SPMSDb"));

var redisConn = builder.Configuration["Redis:Connection"];
if (!string.IsNullOrEmpty(redisConn))
{
    var mux = ConnectionMultiplexer.Connect(redisConn);
    builder.Services.AddSingleton<IConnectionMultiplexer>(mux);
    builder.Services.AddScoped<RedisLockService>();
}

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options => { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true, ValidateAudience = false, ValidateLifetime = true, ValidateIssuerSigningKey = true, ValidIssuer = jwtIssuer, IssuerSigningKey = new SymmetricSecurityKey(keyBytes) });
builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<PaymentGatewayService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var stripeKey = builder.Configuration["Stripe:SecretKey"];
if (!string.IsNullOrEmpty(stripeKey)) Stripe.StripeConfiguration.ApiKey = stripeKey;

var app = builder.Build();
if (!string.IsNullOrEmpty(conn))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Students}/{action=Index}/{id?}");
app.MapControllers();
app.MapPost("/api/payments/webhook", async (HttpContext http, PaymentGatewayService gateway) => Results.Ok(await gateway.HandleWebhookAsync(http)));
app.Run();
