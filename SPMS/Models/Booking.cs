using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public enum BookingType { Hourly, Daily, Monthly }
 public enum BookingStatus { Booked, Active, Completed, Cancelled }

 public class Booking
 {
 [Key]
 public Guid BookingId { get; set; }
 public Guid UserId { get; set; }
 public Guid ParkingSpaceId { get; set; }
 public Guid? SlotId { get; set; }
 public BookingType BookingType { get; set; }
 public DateTime StartTime { get; set; }
 public DateTime EndTime { get; set; }
 public BookingStatus Status { get; set; }
 public decimal Amount { get; set; }
 }
}