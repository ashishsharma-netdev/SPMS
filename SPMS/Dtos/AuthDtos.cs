using SPMS.Models;

namespace SPMS.Dtos
{
 public record RegisterDto(string Name, string Email, string Password, UserRole Role);
 public record LoginDto(string Email, string Password);
 public record UserResponseDto(System.Guid Id, string Name, string Email, UserRole Role);
}