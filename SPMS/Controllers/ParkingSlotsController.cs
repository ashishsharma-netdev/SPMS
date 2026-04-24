using AutoMapper;
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
 public class ParkingSlotsController : ControllerBase
 {
 private readonly AppDbContext _db;
 private readonly IMapper _mapper;
 public ParkingSlotsController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

 [HttpPost("bulk-create")]
 [Authorize]
 public async Task<IActionResult> BulkCreate(Guid parkingId, int count, SlotType slotType = SlotType.Open)
 {
 var parking = await _db.ParkingSpaces.FindAsync(parkingId);
 if (parking == null) return NotFound();
 var start = await _db.ParkingSlots.Where(s => s.ParkingSpaceId == parkingId).CountAsync();
 var list = new List<ParkingSlot>();
 for (int i =1; i <= count; i++) list.Add(new ParkingSlot { SlotId = Guid.NewGuid(), ParkingSpaceId = parkingId, SlotNumber = start + i, SlotType = slotType, IsOccupied = false });
 _db.ParkingSlots.AddRange(list);
 parking.TotalSlots += count;
 parking.AvailableSlots += count;
 await _db.SaveChangesAsync();
 var resp = _mapper.Map<List<ParkingSlotResponseDto>>(list);
 return Ok(resp);
 }

 [HttpGet("by-parking/{parkingId}")]
 public async Task<IActionResult> ByParking(Guid parkingId) => Ok(_mapper.Map<List<ParkingSlotResponseDto>>(await _db.ParkingSlots.Where(s => s.ParkingSpaceId == parkingId).ToListAsync()));

 [HttpGet("available")]
 public async Task<IActionResult> Available(Guid parkingId, SlotType? type = null)
 {
 var q = _db.ParkingSlots.Where(s => s.ParkingSpaceId == parkingId && !s.IsOccupied);
 if (type.HasValue) q = q.Where(s => s.SlotType == type.Value);
 return Ok(await q.ToListAsync());
 }

 [HttpPut("{id}")]
 [Authorize]
 public async Task<IActionResult> Update(Guid id, [FromBody] ParkingSlotCreateDto dto)
 {
 var ex = await _db.ParkingSlots.FindAsync(id);
 if (ex == null) return NotFound();
 ex.SlotType = (SlotType)dto.SlotType;
 await _db.SaveChangesAsync();
 return NoContent();
 }

 [HttpDelete("{id}")]
 [Authorize]
 public async Task<IActionResult> Delete(Guid id)
 {
 var ex = await _db.ParkingSlots.FindAsync(id);
 if (ex == null) return NotFound();
 var parking = await _db.ParkingSpaces.FindAsync(ex.ParkingSpaceId);
 _db.ParkingSlots.Remove(ex);
 if (parking != null) { parking.TotalSlots -=1; parking.AvailableSlots -= ex.IsOccupied ?0 :1; }
 await _db.SaveChangesAsync();
 return NoContent();
 }
 }
}