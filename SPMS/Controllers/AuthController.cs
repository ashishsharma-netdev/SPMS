using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMS.Data;
using SPMS.Dtos;
using SPMS.Models;
using SPMS.Services;
using System.Security.Cryptography;
using System.Text;

namespace SPMS.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class AuthController : ControllerBase
 {
 private readonly AppDbContext _db;
 private readonly JwtTokenService _jwt;
 private readonly IMapper _mapper;

 public AuthController(AppDbContext db, JwtTokenService jwt, IMapper mapper)
 {
 _db = db;
 _jwt = jwt;
 _mapper = mapper;
 }

 [HttpPost("register")]
 public async Task<IActionResult> Register([FromBody] RegisterDto req)
 {
 if (await _db.Users.AnyAsync(u => u.Email == req.Email)) return BadRequest("Email already registered");
 var user = _mapper.Map<User>(req);
 user.Id = Guid.NewGuid();
 user.PasswordHash = HashPassword(req.Password);
 _db.Users.Add(user);
 await _db.SaveChangesAsync();
 var resp = _mapper.Map<UserResponseDto>(user);
 return Ok(resp);
 }

 [HttpPost("login")]
 public async Task<IActionResult> Login([FromBody] LoginDto req)
 {
 var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
 if (user == null) return Unauthorized();
 if (!VerifyPassword(req.Password, user.PasswordHash)) return Unauthorized();
 var token = _jwt.GenerateToken(user);
 return Ok(new { token });
 }

 private static string HashPassword(string password)
 {
 using var sha = SHA256.Create();
 var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
 return Convert.ToBase64String(bytes);
 }
 private static bool VerifyPassword(string password, string hash) => HashPassword(password) == hash;
 }
}
