using FluentValidation;
using SPMS.Dtos;

namespace SPMS.Validators
{
 public class ParkingSpaceCreateValidator : AbstractValidator<ParkingSpaceCreateDto>
 {
 public ParkingSpaceCreateValidator()
 {
 RuleFor(x => x.OwnerId).NotEmpty();
 RuleFor(x => x.Name).NotEmpty();
 RuleFor(x => x.Latitude).InclusiveBetween(-90,90);
 RuleFor(x => x.Longitude).InclusiveBetween(-180,180);
 RuleFor(x => x.TotalSlots).GreaterThanOrEqualTo(0);
 }
 }
}