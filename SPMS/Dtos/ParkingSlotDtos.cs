namespace SPMS.Dtos
{
 public record ParkingSlotCreateDto(System.Guid ParkingSpaceId, int SlotNumber, int SlotType);
 public record ParkingSlotResponseDto(System.Guid SlotId, System.Guid ParkingSpaceId, int SlotNumber, int SlotType, bool IsOccupied);
}