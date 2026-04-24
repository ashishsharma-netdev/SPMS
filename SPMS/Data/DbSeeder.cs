using SPMS.Models;
using System.Security.Cryptography;
using System.Text;

namespace SPMS.Data
{
 public static class DbSeeder
 {
 public static void Seed(AppDbContext db)
 {
 if (db.Users.Any()) return;
 var admin = new User { Id = Guid.NewGuid(), Name = "Admin", Email = "admin@spms.local", PasswordHash = HashPassword("admin"), Role = UserRole.Admin };
 var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "user@spms.local", PasswordHash = HashPassword("user"), Role = UserRole.User };
 db.Users.Add(admin);
 db.Users.Add(user);

 var ps = new ParkingSpace { ParkingSpaceId = Guid.NewGuid(), Name = "Central Mall", Address = "123 Main St", Latitude =12.9716, Longitude =77.5946, TotalSlots =100, AvailableSlots =100, IsActive = true };
 db.ParkingSpaces.Add(ps);

 db.SaveChanges();
 }

 private static string HashPassword(string password)
 {
 using var sha = SHA256.Create();
 var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
 return Convert.ToBase64String(bytes);
 }
 }
}