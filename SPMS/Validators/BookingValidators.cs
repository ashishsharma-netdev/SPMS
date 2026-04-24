using FluentValidation;
using SPMS.Dtos;

namespace SPMS.Validators
{
 public class BookingCreateValidator : AbstractValidator<BookingCreateDto>
 {
 public BookingCreateValidator()
 {
 RuleFor(x => x.ParkingSpaceId).NotEmpty();
 RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
 RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
 }
 }
}