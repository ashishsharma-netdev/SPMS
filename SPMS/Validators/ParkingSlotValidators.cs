using FluentValidation;
using SPMS.Dtos;

namespace SPMS.Validators
{
 public class ParkingSlotCreateValidator : AbstractValidator<ParkingSlotCreateDto>
 {
 public ParkingSlotCreateValidator()
 {
 RuleFor(x => x.ParkingSpaceId).NotEmpty();
 RuleFor(x => x.SlotNumber).GreaterThan(0);
 RuleFor(x => x.SlotType).InclusiveBetween(0,2);
 }
 }
}