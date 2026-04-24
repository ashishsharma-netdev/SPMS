using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public enum PriceType { Hourly, Daily, Monthly }

 public class Pricing
 {
 [Key]
 public Guid PricingId { get; set; }
 public Guid ParkingSpaceId { get; set; }
 public string VehicleType { get; set; }
 public PriceType PriceType { get; set; }
 public decimal Amount { get; set; }
 public decimal GST { get; set; }
 }
}