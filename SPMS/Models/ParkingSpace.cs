using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public class ParkingSpace
 {
 [Key]
 public Guid ParkingSpaceId { get; set; }
 public Guid OwnerId { get; set; }
 public string Name { get; set; }
 public double Latitude { get; set; }
 public double Longitude { get; set; }
 public string Address { get; set; }
 public int TotalSlots { get; set; }
 public int AvailableSlots { get; set; }
 public double AreaInSqFt { get; set; }
 public DateTime? StartDate { get; set; }
 public DateTime? EndDate { get; set; }
 public bool IsActive { get; set; }
 }
}