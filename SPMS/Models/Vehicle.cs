using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public class Vehicle
 {
 [Key]
 public Guid VehicleId { get; set; }
 public Guid OwnerId { get; set; }
 public string VehicleNumber { get; set; }
 public string VehicleType { get; set; }
 public string Brand { get; set; }
 public bool IsDefault { get; set; }
 }
}