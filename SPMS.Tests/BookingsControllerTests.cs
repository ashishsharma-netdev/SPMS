using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMS.Controllers;
using SPMS.Data;
using SPMS.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SPMS.Tests
{
 public class BookingsControllerTests
 {
 private AppDbContext CreateDbContext()
 {
 var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
 return new AppDbContext(options);
 }

 [Fact]
 public async Task CreateBooking_Decrements_AvailableSlots()
 {
 var db = CreateDbContext();
 var ps = new ParkingSpace { ParkingSpaceId = Guid.NewGuid(), Name = "P", Address = "A", Latitude =0, Longitude =0, TotalSlots =1, AvailableSlots =1, IsActive = true };
 db.ParkingSpaces.Add(ps);
 await db.SaveChangesAsync();

 var controller = new BookingsController(db);
 var booking = new Booking { UserId = Guid.NewGuid(), ParkingSpaceId = ps.ParkingSpaceId, StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1), BookingType = BookingType.Hourly, Amount =10 };
 var res = await controller.Create(booking) as CreatedAtActionResult;
 Assert.NotNull(res);
 var updated = await db.ParkingSpaces.FindAsync(ps.ParkingSpaceId);
 Assert.Equal(0, updated.AvailableSlots);
 }
 }
}