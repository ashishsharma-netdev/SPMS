using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public class Caretaker
 {
 [Key]
 public Guid CaretakerId { get; set; }
 public string Name { get; set; }
 public string Phone { get; set; }
 public Guid AssignedParkingSpaceId { get; set; }
 }
}