using Microsoft.EntityFrameworkCore;
using Moq;
using SPMS.Data;
using SPMS.Models;
using SPMS.Services;
using Stripe;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SPMS.Tests
{
 public class PaymentGatewayServiceTests
 {
 private AppDbContext CreateDbContext()
 {
 var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
 return new AppDbContext(options);
 }

 [Fact]
 public async Task ProcessEvent_PaymentIntentSucceeded_UpdatesPaymentAndBooking()
 {
 var db = CreateDbContext();
 var paymentId = Guid.NewGuid();
 var bookingId = Guid.NewGuid();
 var payment = new Payment { PaymentId = paymentId, BookingId = bookingId, Amount =100, Status = PaymentStatus.Initiated };
 var booking = new Booking { BookingId = bookingId, UserId = Guid.NewGuid(), ParkingSpaceId = Guid.NewGuid(), StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1), BookingType = BookingType.Hourly, Amount =100, Status = BookingStatus.Booked };
 db.Payments.Add(payment);
 db.Bookings.Add(booking);
 await db.SaveChangesAsync();

 var svc = new PaymentGatewayService(new Moq.Mock<Microsoft.Extensions.Configuration.IConfiguration>().Object, db);
 var pi = new PaymentIntent { Id = "pi_123" };
 pi.Metadata = new System.Collections.Generic.Dictionary<string, string> { { "paymentId", paymentId.ToString() } };
 var evt = new Event { Id = "evt1", Type = Events.PaymentIntentSucceeded, Data = new EventData { Object = pi } };
 var res = await svc.ProcessEvent(evt);
 var updated = await db.Payments.FindAsync(paymentId);
 var updatedBooking = await db.Bookings.FindAsync(bookingId);
 Assert.Equal(PaymentStatus.Success, updated.Status);
 Assert.Equal("pi_123", updated.TransactionId);
 Assert.Equal(BookingStatus.Active, updatedBooking.Status);
 }
 }
}