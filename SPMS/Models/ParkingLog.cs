using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public class ParkingLog
 {
 [Key]
 public Guid ParkingLogId { get; set; }
 public Guid BookingId { get; set; }
 public DateTime EntryTime { get; set; }
 public DateTime? ExitTime { get; set; }
 public string? CheckCode { get; set; }
 }
}