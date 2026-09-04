using System.ComponentModel.DataAnnotations;

namespace SPMS.Models;

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, StringLength(20)]
    public string Mobile { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required, StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Course { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string State { get; set; } = string.Empty;

    [Required, RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must contain exactly 6 digits.")]
    public string Pincode { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
