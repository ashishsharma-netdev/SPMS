using FluentValidation;
using SPMS.Dtos;

namespace SPMS.Validators
{
 public class PaymentInitiateValidator : AbstractValidator<PaymentInitiateDto>
 {
 public PaymentInitiateValidator()
 {
 RuleFor(x => x.BookingId).NotEmpty();
 RuleFor(x => x.Amount).GreaterThan(0);
 RuleFor(x => x.Method).NotEmpty();
 }
 }
}