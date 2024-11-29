using Ardalis.Result;

namespace RackLogic.UseCases.Users.Create;
public record RegisterUserCommand 
  (string FirstName, string LastName, string EmailAddress, string PhoneNumber, string Postcode, string Password, bool OptOutRackLogicMarketing, string Role, Guid? InviteId)
  : Ardalis.SharedKernel.ICommand<Result>;
