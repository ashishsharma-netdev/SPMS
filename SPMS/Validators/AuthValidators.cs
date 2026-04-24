using FluentValidation;
using SPMS.Dtos;

namespace SPMS.Validators
{
 public class RegisterValidator : AbstractValidator<RegisterDto>
 {
 public RegisterValidator()
 {
 RuleFor(x => x.Name).NotEmpty();
 RuleFor(x => x.Email).NotEmpty().EmailAddress();
 RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
 }
 }

 public class LoginValidator : AbstractValidator<LoginDto>
 {
 public LoginValidator()
 {
 RuleFor(x => x.Email).NotEmpty().EmailAddress();
 RuleFor(x => x.Password).NotEmpty();
 }
 }
}