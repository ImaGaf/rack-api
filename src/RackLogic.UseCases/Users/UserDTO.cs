namespace RackLogic.UseCases.Users;
public record UserDTO(string FirstName, string LastName, string? Email, string? PhoneNumber, string RegisteredRole, bool IsPaid);
