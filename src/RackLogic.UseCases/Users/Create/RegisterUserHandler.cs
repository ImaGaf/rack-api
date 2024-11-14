using System.ComponentModel.DataAnnotations;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.Create;
public class RegisterUserHandler(
  UserManager<ApplicationUser> _userManager,
  IUserStore<ApplicationUser> _userStore) 
  : ICommandHandler<RegisterUserCommand, Result>
{
  private readonly EmailAddressAttribute _emailAddressAttribute = new();
  public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
  {
    var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
    var email = request.EmailAddress;

    if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
    {
      var invalidEmail = _userManager.ErrorDescriber.InvalidEmail(email);
      return Result.Invalid(new ValidationError(invalidEmail.Description));
    }

    var oldUser = await _userManager.FindByNameAsync(email);
    if (oldUser is not null)
    {
      return Result.Invalid(new ValidationError("This email already is already in use, either re enter your email or go back to the login page and select Forgot Password to reset your password."));
    }

    var user = new ApplicationUser();
    user.Email = email;
    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.Postcode = request.Postcode;
    user.PhoneNumber = request.PhoneNumber;
    user.OptOutRackLogicMarketing = request.OptOutRackLogicMarketing;
    user.UserName = request.EmailAddress;
    user.EmailConfirmed = true;
    var result = await _userManager.CreateAsync(user, request.Password);
    var errors = result.Errors.Select(x => x.Description);

    if (!result.Succeeded)
    {
      return Result.Error(new ErrorList(errors));
    }

    //await SendConfirmationEmailAsync(user, userManager, context, email);
    return Result.Success();
  }
}
