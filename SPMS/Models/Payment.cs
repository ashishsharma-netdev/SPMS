using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public enum PaymentStatus { Initiated, Success, Failed, Refunded }

 public class Payment
 {
 [Key]
 public Guid PaymentId { get; set; }
 public Guid BookingId { get; set; }
 public decimal Amount { get; set; }
 public string PaymentMethod { get; set; }
 public PaymentStatus Status { get; set; }
 public string TransactionId { get; set; }
 }
}