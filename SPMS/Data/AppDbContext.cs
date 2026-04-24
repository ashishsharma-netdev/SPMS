using Microsoft.EntityFrameworkCore;
using SPMS.Models;

namespace SPMS.Data
{
 public class AppDbContext : DbContext
 {
 public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

 public DbSet<User> Users { get; set; }
 public DbSet<Vehicle> Vehicles { get; set; }
 public DbSet<ParkingSpace> ParkingSpaces { get; set; }
 public DbSet<ParkingSlot> ParkingSlots { get; set; }
 public DbSet<Booking> Bookings { get; set; }
 public DbSet<Pricing> Pricings { get; set; }
 public DbSet<Payment> Payments { get; set; }
 public DbSet<Caretaker> Caretakers { get; set; }
 public DbSet<ParkingLog> ParkingLogs { get; set; }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
 modelBuilder.Entity<ParkingSlot>().Property(p => p.RowVersion).IsRowVersion();
 base.OnModelCreating(modelBuilder);
 }
 }
}