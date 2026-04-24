using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMS.Data;
using SPMS.Dtos;
using SPMS.Models;
using SPMS.Services;

namespace SPMS.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class BookingsController : ControllerBase
 {
 private readonly AppDbContext _db;
 private readonly IMapper _mapper;
 private readonly RedisLockService? _lockService;
 public BookingsController(AppDbContext db, IMapper mapper, RedisLockService? lockService = null) { _db = db; _mapper = mapper; _lockService = lockService; }

 [HttpPost]
 [Authorize]
 public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
 {
 var req = _mapper.Map<Booking>(dto);
 var parking = await _db.ParkingSpaces.FindAsync(req.ParkingSpaceId);
 if (parking == null || !parking.IsActive) return BadRequest("Parking not available");

 var lockKey = $"booking:{req.ParkingSpaceId}";
 var locked = true;
 if (_lockService != null) locked = await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromMinutes(2));
 if (!locked) return Conflict("Parking is busy");

 try
 {
 if (req.SlotId.HasValue)
 {
 var slot = await _db.ParkingSlots.FirstOrDefaultAsync(s => s.SlotId == req.SlotId.Value);
 if (slot == null) return BadRequest("Invalid slot");
 if (slot.IsOccupied) return BadRequest("Slot already occupied");
 slot.IsOccupied = true;
 try { await _db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return Conflict("Slot reserved by another"); }
 }
 else
 {
 if (parking.AvailableSlots <=0) return BadRequest("No slots available");
 parking.AvailableSlots -=1;
 }
 req.BookingId = Guid.NewGuid();
 req.Status = BookingStatus.Booked;
 _db.Bookings.Add(req);
 await _db.SaveChangesAsync();
 var resp = _mapper.Map<BookingResponseDto>(req);
 return CreatedAtAction(nameof(GetById), new { id = req.BookingId }, resp);
 }
 finally { if (_lockService != null) await _lockService.ReleaseLockAsync(lockKey); }
 }

 [HttpGet("availability")]
 public async Task<IActionResult> CheckAvailability(Guid parkingId, SlotType? type = null)
 {
 var parking = await _db.ParkingSpaces.FindAsync(parkingId);
 if (parking == null) return NotFound();
 var q = _db.ParkingSlots.Where(s => s.ParkingSpaceId == parkingId && !s.IsOccupied);
 if (type.HasValue) q = q.Where(s => s.SlotType == type.Value);
 var count = await q.CountAsync();
 return Ok(new { parking.TotalSlots, Available = count });
 }

 [HttpGet("user")]
 [Authorize]
 public async Task<IActionResult> GetForUser()
 {
 var userId = GetUserId();
 return Ok(await _db.Bookings.Where(b => b.UserId == userId).ToListAsync());
 }

 [HttpGet("{id}")]
 [Authorize]
 public async Task<IActionResult> GetById(Guid id) => Ok(await _db.Bookings.FindAsync(id));

 [HttpPut("cancel/{id}")]
 [Authorize]
 public async Task<IActionResult> Cancel(Guid id)
 {
 var b = await _db.Bookings.FindAsync(id);
 if (b == null) return NotFound();
 if (b.Status == BookingStatus.Cancelled) return BadRequest("Already cancelled");
 b.Status = BookingStatus.Cancelled;
 var parking = await _db.ParkingSpaces.FindAsync(b.ParkingSpaceId);
 if (b.SlotId.HasValue)
 {
 var slot = await _db.ParkingSlots.FindAsync(b.SlotId.Value);
 if (slot != null)
 {
 slot.IsOccupied = false;
 }
 }
 else
 {
 if (parking != null) parking.AvailableSlots +=1;
 }
 await _db.SaveChangesAsync();
 return NoContent();
 }

 private Guid GetUserId()
 {
 var sid = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
 if (Guid.TryParse(sid, out var id)) return id;
 throw new Exception("Invalid user");
 }
 }
}