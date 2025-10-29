using System.ComponentModel.DataAnnotations;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Users.Create;

public class RegisterUserHandler(
  UserManager<ApplicationUser> _userManager,
  RoleManager<IdentityRole<Guid>> _roleManager,
  IInvitationService _invitationService)
  : ICommandHandler<RegisterUserCommand, Result<Guid>>
{
  private readonly EmailAddressAttribute _emailAddressAttribute = new();

  public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
  {
    var email = request.EmailAddress.Trim();

    if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
    {
      var invalidEmail = _userManager.ErrorDescriber.InvalidEmail(email);
      return Result.Invalid(new ValidationError(invalidEmail.Description));
    }

    var oldUser = await _userManager.FindByEmailAsync(email);
    if (oldUser is not null)
    {
      return Result.Invalid(new ValidationError("This email already is already in use, either re enter your email or go back to the login page and select Forgot Password to reset your password."));
    }

    if (!await _roleManager.RoleExistsAsync(request.Role)) return Result.Invalid(new ValidationError("Role invalid."));

    var user = new ApplicationUser();
    user.Email = email;
    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.Postcode = request.Postcode;
    user.PhoneNumber = request.PhoneNumber;
    user.OptOutRackLogicMarketing = request.OptOutRackLogicMarketing;
    user.UserName = email;
    user.EmailConfirmed = true;
    user.RegisteredRole = request.Role;
    var result = await _userManager.CreateAsync(user, request.Password);
    var errors = result.Errors.Select(x => x.Description);

    if (!result.Succeeded)
    {
      return Result.Error(new ErrorList(errors));
    }

    if (request.WarehouseId != null)
    {
      await _invitationService.AcceptInvitationAsync(request.EmailAddress, request.WarehouseId.Value, user, cancellationToken);
    }

    return Result.Success(user.Id);
  }
}
