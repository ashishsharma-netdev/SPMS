using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMS.Data;
using SPMS.Dtos;
using SPMS.Models;

namespace SPMS.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class ParkingLogsController : ControllerBase
 {
 private readonly AppDbContext _db;
 public ParkingLogsController(AppDbContext db) { _db = db; }

 [HttpPost("entry")]
 [Authorize]
 public async Task<IActionResult> Entry([FromBody] EntryRequestDto req)
 {
 var booking = await _db.Bookings.FindAsync(req.BookingId);
 if (booking == null) return NotFound();
 if (booking.Status != BookingStatus.Active && booking.Status != BookingStatus.Booked) return BadRequest("Booking not valid for entry");

 // create parking log and mark booking active
 var log = new ParkingLog { ParkingLogId = Guid.NewGuid(), BookingId = booking.BookingId, EntryTime = DateTime.UtcNow, CheckCode = GenerateCode() };
 _db.ParkingLogs.Add(log);
 booking.Status = BookingStatus.Active;
 await _db.SaveChangesAsync();
 return Ok(new { log.ParkingLogId, log.CheckCode, log.EntryTime });
 }

 [HttpPost("exit")]
 [Authorize]
 public async Task<IActionResult> Exit([FromBody] ExitRequestDto req)
 {
 var log = await _db.ParkingLogs.FirstOrDefaultAsync(l => l.ParkingLogId == req.ParkingLogId);
 if (log == null) return NotFound();
 if (log.ExitTime != null) return BadRequest("Already exited");
 log.ExitTime = DateTime.UtcNow;
 // calculate overstay
 var booking = await _db.Bookings.FindAsync(log.BookingId);
 if (booking != null)
 {
 var allowed = booking.EndTime;
 var overstay = log.ExitTime.Value > allowed ? (log.ExitTime.Value - allowed).TotalMinutes :0;
 // For demo: if overstay >0 create a payment record to collect extra charges
 if (overstay >0)
 {
 var extra = Convert.ToDecimal(Math.Ceiling(overstay/60));
 var p = new Payment { PaymentId = Guid.NewGuid(), BookingId = booking.BookingId, Amount = extra, PaymentMethod = "Overstay", Status = PaymentStatus.Initiated };
 _db.Payments.Add(p);
 }
 booking.Status = BookingStatus.Completed;
 }
 await _db.SaveChangesAsync();
 return Ok(new { log.ExitTime });
 }

 [HttpGet("active")]
 public async Task<IActionResult> Active() => Ok(await _db.ParkingLogs.Where(l => l.ExitTime == null).ToListAsync());

 [HttpGet("history")]
 public async Task<IActionResult> History(Guid? userId)
 {
 var q = _db.ParkingLogs.AsQueryable();
 if (userId.HasValue)
 {
 q = from l in _db.ParkingLogs
 join b in _db.Bookings on l.BookingId equals b.BookingId
 where b.UserId == userId.Value
 select l;
 }
 return Ok(await q.ToListAsync());
 }

 private static string GenerateCode()
 {
 var rng = new Random();
 return rng.Next(100000,999999).ToString();
 }
 }

 public record EntryRequestDto(System.Guid BookingId);
 public record ExitRequestDto(System.Guid ParkingLogId);
}