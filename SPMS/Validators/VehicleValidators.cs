using FluentValidation;
using SPMS.Dtos;

namespace SPMS.Validators
{
 public class VehicleCreateValidator : AbstractValidator<VehicleCreateDto>
 {
 public VehicleCreateValidator()
 {
 RuleFor(x => x.OwnerId).NotEmpty();
 RuleFor(x => x.VehicleNumber).NotEmpty().MaximumLength(50);
 RuleFor(x => x.VehicleType).NotEmpty();
 RuleFor(x => x.Brand).NotEmpty();
 }
 }
}