using System.ComponentModel.DataAnnotations;

namespace SPMS.Dtos
{
 public record BookingCreateDto
 (
 Guid ParkingSpaceId,
 Guid? SlotId,
 DateTime StartTime,
 DateTime EndTime,
 decimal Amount,
 int BookingType
 );

 public record BookingResponseDto
 (
 Guid BookingId,
 Guid UserId,
 Guid ParkingSpaceId,
 Guid? SlotId,
 int BookingType,
 DateTime StartTime,
 DateTime EndTime,
 int Status,
 decimal Amount
 );
}