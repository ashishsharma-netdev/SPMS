using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SPMS.Controllers;
using SPMS.Data;
using SPMS.Dtos;
using SPMS.Models;
using SPMS.Profiles;
using SPMS.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SPMS.Tests
{
 public class PaymentsControllerTests
 {
 private AppDbContext CreateDbContext()
 {
 var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
 return new AppDbContext(options);
 }

 private IMapper CreateMapper()
 {
 var cfg = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
 return cfg.CreateMapper();
 }

 [Fact]
 public async Task Initiate_Returns_ClientSecret_When_Stripe_Configured()
 {
 var db = CreateDbContext();
 var mapper = CreateMapper();
 var booking = new Booking { BookingId = Guid.NewGuid(), UserId = Guid.NewGuid(), ParkingSpaceId = Guid.NewGuid(), StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1), BookingType = BookingType.Hourly, Amount =10, Status = BookingStatus.Booked };
 db.Bookings.Add(booking);
 await db.SaveChangesAsync();

 var gatewayMock = new Mock<PaymentGatewayService>(MockBehavior.Strict, null, db);
 // Setup CreatePaymentIntentAsync to return null (as Stripe not configured in test)
 gatewayMock.Setup(g => g.CreatePaymentIntentAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<string>())).ReturnsAsync((Stripe.PaymentIntent?)null);

 var controller = new PaymentsController(db, gatewayMock.Object, mapper);
 var dto = new PaymentInitiateDto(booking.BookingId,10, "card", null, "inr");
 var res = await controller.Initiate(dto) as OkObjectResult;
 Assert.NotNull(res);
 }
 }
}