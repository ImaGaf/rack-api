using Ardalis.Result;

namespace RackLogic.UseCases.Users.Create;
public record RegisterUserCommand 
  (string FirstName, string LastName, string EmailAddress, string PhoneNumber, string Postcode, string Password, bool OptOutRackLogicMarketing, List<string> Roles)
  : Ardalis.SharedKernel.ICommand<Result>;
