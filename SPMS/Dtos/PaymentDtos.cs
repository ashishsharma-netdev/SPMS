namespace SPMS.Dtos
{
 public record PaymentInitiateDto(Guid BookingId, decimal Amount, string Method, string? Provider, string? Currency);
 public record PaymentResponseDto(System.Guid PaymentId, System.Guid BookingId, decimal Amount, string PaymentMethod, int Status, string TransactionId);
}