using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public enum SlotType { Covered, Open, EV }

 public class ParkingSlot
 {
 [Key]
 public Guid SlotId { get; set; }
 public Guid ParkingSpaceId { get; set; }
 public int SlotNumber { get; set; }
 public SlotType SlotType { get; set; }
 public bool IsOccupied { get; set; }

 [Timestamp]
 public byte[]? RowVersion { get; set; }
 }
}