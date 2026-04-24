using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMS.Data;
using SPMS.Dtos;
using SPMS.Models;
using SPMS.Services;
using Stripe;

namespace SPMS.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class PaymentsController : ControllerBase
 {
 private readonly AppDbContext _db;
 private readonly PaymentGatewayService _gateway;
 private readonly RedisLockService? _lockService;
 private readonly IMapper _mapper;
 public PaymentsController(AppDbContext db, PaymentGatewayService gateway, IMapper mapper, RedisLockService? lockService = null) { _db = db; _gateway = gateway; _mapper = mapper; _lockService = lockService; }

 // Initiate a payment (real Stripe integration when configured)
 [HttpPost("initiate")]
 [Authorize]
 public async Task<IActionResult> Initiate([FromBody] PaymentInitiateDto dto)
 {
 var booking = await _db.Bookings.FindAsync(dto.BookingId);
 if (booking == null) return NotFound();
 if (booking.Status != BookingStatus.Booked) return BadRequest("Booking not in payable state");

 var lockKey = $"booking:{dto.BookingId}";
 var locked = true;
 if (_lockService != null) locked = await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromMinutes(5));
 if (!locked) return Conflict("Booking is being processed");

 try
 {
 var p = _mapper.Map<Payment>(dto);
 p.PaymentId = Guid.NewGuid();
 p.Status = PaymentStatus.Initiated;
 p.TransactionId = string.Empty;
 _db.Payments.Add(p);
 await _db.SaveChangesAsync();

 var intent = await _gateway.CreatePaymentIntentAsync(p.PaymentId, p.Amount, dto.Currency ?? "inr");
 if (intent is PaymentIntent si) return Ok(new { paymentId = p.PaymentId, clientSecret = si.ClientSecret });
 var gatewayInfo = new { Provider = dto.Provider ?? "Stripe", CheckoutUrl = $"https://payments.example/{p.PaymentId}" };
 return Ok(new { paymentId = p.PaymentId, gateway = gatewayInfo });
 }
 finally { if (_lockService != null) await _lockService.ReleaseLockAsync(lockKey); }
 }

 // Verify payment (stub)
 [HttpPost("verify")]
 [Authorize]
 public async Task<IActionResult> Verify([FromBody] PaymentVerifyRequestDto req)
 {
 var p = await _db.Payments.FindAsync(req.PaymentId);
 if (p == null) return NotFound();
 // simulate verification
 if (req.Success)
 {
 p.Status = PaymentStatus.Success;
 p.TransactionId = req.TransactionId ?? "txn_demo";
 var booking = await _db.Bookings.FindAsync(p.BookingId);
 if (booking != null) booking.Status = BookingStatus.Active;
 }
 else
 {
 p.Status = PaymentStatus.Failed;
 }
 await _db.SaveChangesAsync();
 return Ok(p);
 }

 [HttpGet("by-booking/{bookingId}")]
 public async Task<IActionResult> ByBooking(Guid bookingId) => Ok(await _db.Payments.Where(x => x.BookingId == bookingId).ToListAsync());

 [HttpPost("refund")]
 [Authorize]
 public async Task<IActionResult> Refund([FromBody] RefundRequestDto req)
 {
 var p = await _db.Payments.FindAsync(req.PaymentId);
 if (p == null) return NotFound();
 // stub refund
 p.Status = PaymentStatus.Refunded;
 await _db.SaveChangesAsync();
 return Ok(p);
 }
 }
}