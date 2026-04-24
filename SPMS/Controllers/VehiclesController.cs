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
 public class VehiclesController : ControllerBase
 {
 private readonly AppDbContext _db;
 private readonly IMapper _mapper;
 public VehiclesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

 [HttpPost]
 [Authorize]
 public async Task<IActionResult> Create([FromBody] VehicleCreateDto dto)
 {
 var v = _mapper.Map<Vehicle>(dto);
 v.VehicleId = Guid.NewGuid();
 _db.Vehicles.Add(v);
 await _db.SaveChangesAsync();
 var resp = _mapper.Map<VehicleResponseDto>(v);
 return CreatedAtAction(nameof(GetById), new { id = v.VehicleId }, resp);
 }

 [HttpGet("user")]
 [Authorize]
 public async Task<IActionResult> GetByUser()
 {
 var userId = GetUserId();
 var list = await _db.Vehicles.Where(x => x.OwnerId == userId).ToListAsync();
 var resp = _mapper.Map<List<VehicleResponseDto>>(list);
 return Ok(resp);
 }

 [HttpGet("{id}")]
 [Authorize]
 public async Task<IActionResult> GetById(Guid id)
 {
 var v = await _db.Vehicles.FindAsync(id);
 if (v == null) return NotFound();
 return Ok(_mapper.Map<VehicleResponseDto>(v));
 }

 [HttpPut("{id}")]
 [Authorize]
 public async Task<IActionResult> Update(Guid id, [FromBody] VehicleCreateDto dto)
 {
 var ex = await _db.Vehicles.FindAsync(id);
 if (ex == null) return NotFound();
 ex.Brand = dto.Brand;
 ex.VehicleNumber = dto.VehicleNumber;
 ex.VehicleType = dto.VehicleType;
 await _db.SaveChangesAsync();
 return NoContent();
 }

 [HttpDelete("{id}")]
 [Authorize]
 public async Task<IActionResult> Delete(Guid id)
 {
 var ex = await _db.Vehicles.FindAsync(id);
 if (ex == null) return NotFound();
 _db.Vehicles.Remove(ex);
 await _db.SaveChangesAsync();
 return NoContent();
 }

 [HttpPut("set-default/{id}")]
 [Authorize]
 public async Task<IActionResult> SetDefault(Guid id)
 {
 var v = await _db.Vehicles.FindAsync(id);
 if (v == null) return NotFound();
 var userId = v.OwnerId;
 var list = await _db.Vehicles.Where(x => x.OwnerId == userId).ToListAsync();
 foreach (var item in list) item.IsDefault = false;
 v.IsDefault = true;
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