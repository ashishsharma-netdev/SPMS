using System.ComponentModel.DataAnnotations;

namespace SPMS.Models
{
 public enum UserRole { Admin, Owner, Caretaker, User }

 public class User
 {
 [Key]
 public Guid Id { get; set; }
 public string Name { get; set; }
 public string Email { get; set; }
 public string PasswordHash { get; set; }
 public UserRole Role { get; set; }
 }
}