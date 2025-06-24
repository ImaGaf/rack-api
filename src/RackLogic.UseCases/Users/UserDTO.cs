namespace RackLogic.UseCases.Users;
public record UserDTO(
  Guid Id,
  string FirstName,
  string LastName,
  string? Email,
  string? PhoneNumber,
  string RegisteredRole,
  bool IsPaid,
  bool IsTransactionGenerated,
  string? SubscriptionId,
  DateTime? ExpiryTime);
