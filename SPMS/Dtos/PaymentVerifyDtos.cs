namespace SPMS.Dtos
{
 public record PaymentVerifyRequestDto(System.Guid PaymentId, bool Success, string? TransactionId);
 public record RefundRequestDto(System.Guid PaymentId);
}