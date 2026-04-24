namespace SPMS.Dtos
{
 public record VehicleCreateDto(System.Guid OwnerId, string VehicleNumber, string VehicleType, string Brand, bool IsDefault);
 public record VehicleResponseDto(System.Guid VehicleId, System.Guid OwnerId, string VehicleNumber, string VehicleType, string Brand, bool IsDefault);
}