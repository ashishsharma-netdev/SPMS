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
 public class ParkingSpacesController : ControllerBase
 {
 private readonly AppDbContext _db;
 private readonly IMapper _mapper;
 public ParkingSpacesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

 [HttpPost]
 [Authorize]
 public async Task<IActionResult> Create([FromBody] ParkingSpaceCreateDto dto)
 {
 var entity = _mapper.Map<ParkingSpace>(dto);
 entity.ParkingSpaceId = Guid.NewGuid();
 entity.AvailableSlots = dto.TotalSlots;
 _db.ParkingSpaces.Add(entity);
 await _db.SaveChangesAsync();
 var resp = _mapper.Map<ParkingSpaceResponseDto>(entity);
 return CreatedAtAction(nameof(GetById), new { id = entity.ParkingSpaceId }, resp);
 }

 [HttpGet]
 public async Task<IActionResult> GetAll()
 {
 var list = await _db.ParkingSpaces.ToListAsync();
 return Ok(_mapper.Map<List<ParkingSpaceResponseDto>>(list));
 }

 [HttpGet("search")]
 public async Task<IActionResult> Search([FromQuery] double lat, [FromQuery] double lng, [FromQuery] double radiusKm =5)
 {
 var all = await _db.ParkingSpaces.ToListAsync();
 var res = all.Where(p => Distance(lat, lng, p.Latitude, p.Longitude) <= radiusKm).ToList();
 var resp = _mapper.Map<List<ParkingSpaceResponseDto>>(res);
 return Ok(resp);
 }

 [HttpGet("{id}")]
 public async Task<IActionResult> GetById(Guid id)
 {
 var p = await _db.ParkingSpaces.FindAsync(id);
 if (p == null) return NotFound();
 return Ok(_mapper.Map<ParkingSpaceResponseDto>(p));
 }

 private static double Distance(double lat1, double lon1, double lat2, double lon2)
 {
 double R =6371;
 var dLat = ToRad(lat2 - lat1);
 var dLon = ToRad(lon2 - lon1);
 var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) + Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) * Math.Sin(dLon/2) * Math.Sin(dLon/2);
 var c =2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
 return R * c;
 }
 private static double ToRad(double deg) => deg * (Math.PI/180);
 }
}