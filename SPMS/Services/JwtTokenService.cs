using Microsoft.IdentityModel.Tokens;
using SPMS.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SPMS.Services
{
 public class JwtTokenService
 {
 private readonly IConfiguration _config;
 public JwtTokenService(IConfiguration config) { _config = config; }

 public string GenerateToken(User user)
 {
 var jwtKey = _config["Jwt:Key"] ?? "ThisIsADevSecretKey_ForDemoPurposesOnly!ChangeInProd";
 var jwtIssuer = _config["Jwt:Issuer"] ?? "SPMS";
 var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
 var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

 var claims = new[] {
 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
 new Claim(ClaimTypes.Role, user.Role.ToString()),
 new Claim(ClaimTypes.Email, user.Email)
 };

 var token = new JwtSecurityToken(issuer: jwtIssuer, claims: claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
 return new JwtSecurityTokenHandler().WriteToken(token);
 }
 }
}