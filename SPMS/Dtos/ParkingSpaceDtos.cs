namespace SPMS.Dtos
{
 public record ParkingSpaceCreateDto(System.Guid OwnerId, string Name, double Latitude, double Longitude, string Address, int TotalSlots, double AreaInSqFt, bool IsActive);
 public record ParkingSpaceResponseDto(System.Guid ParkingSpaceId, System.Guid OwnerId, string Name, double Latitude, double Longitude, string Address, int TotalSlots, int AvailableSlots, double AreaInSqFt, System.DateTime? StartDate, System.DateTime? EndDate, bool IsActive);
}