using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SPMS.Data;
using SPMS.Models;
using Stripe;

namespace SPMS.Services
{
 public class PaymentGatewayService
 {
 private readonly IConfiguration _config;
 private readonly AppDbContext _db;
 public PaymentGatewayService(IConfiguration config, AppDbContext db) { _config = config; _db = db; }

 public async Task<PaymentIntent?> CreatePaymentIntentAsync(Guid paymentId, decimal amount, string currency = "inr")
 {
 // create Stripe PaymentIntent (stub if no key configured)
 var sk = _config["Stripe:SecretKey"];
 if (string.IsNullOrEmpty(sk)) return null;
 var service = new PaymentIntentService();
 var options = new PaymentIntentCreateOptions
 {
 Amount = (long)(amount *100),
 Currency = currency,
 Metadata = new Dictionary<string, string> { { "paymentId", paymentId.ToString() } }
 };
 try
 {
 var pi = await service.CreateAsync(options);
 return pi;
 }
 catch
 {
 return null;
 }
 }

 public async Task<object> HandleWebhookAsync(HttpContext http)
 {
 var json = await new StreamReader(http.Request.Body).ReadToEndAsync();
 var sig = http.Request.Headers["Stripe-Signature"].ToString();
 var secret = _config["Stripe:WebhookSecret"];
 try
 {
 var evt = EventUtility.ConstructEvent(json, sig, secret);
 if (evt.Type == Events.PaymentIntentSucceeded)
 {
 var pi = evt.Data.Object as PaymentIntent;
 var paymentId = Guid.Empty;
 if (pi.Metadata != null && pi.Metadata.ContainsKey("paymentId")) Guid.TryParse(pi.Metadata["paymentId"], out paymentId);
 if (paymentId != Guid.Empty)
 {
 var p = await _db.Payments.FirstOrDefaultAsync(x => x.PaymentId == paymentId);
 if (p != null) { p.Status = PaymentStatus.Success; p.TransactionId = pi.Id; var booking = await _db.Bookings.FindAsync(p.BookingId); if (booking != null) booking.Status = BookingStatus.Active; await _db.SaveChangesAsync(); }
 }
 }
 return new { received = true };
 }
 catch (Exception ex)
 {
 return new { error = ex.Message };
 }
 }
 }
}