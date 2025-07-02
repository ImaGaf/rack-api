using System.ComponentModel.DataAnnotations;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.Create;

public class RegisterUserHandler(
  UserManager<ApplicationUser> _userManager,
  RoleManager<IdentityRole<Guid>> _roleManager,
  IUserStore<ApplicationUser> _userStore,
  IRepositoryWithExpression<Invitation> _invitationsRepository,
  IRepositoryWithExpression<WarehouseFinalUses> _warehouseFinalUseRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository)
  : ICommandHandler<RegisterUserCommand, Result<Guid>>
{
  private readonly EmailAddressAttribute _emailAddressAttribute = new();

  public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
  {
    var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
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
      await HandleInvitationAsync(request.EmailAddress, request.WarehouseId.Value, user, cancellationToken);
    }
    // TODO: Send Email create account.
    //await SendConfirmationEmailAsync(user, userManager, context, email);
    return Result.Success(user.Id);
  }

  private async Task HandleInvitationAsync(string emailInvited, Guid warehouseId, ApplicationUser user, CancellationToken cancellationToken)
  {
    if (user.RegisteredRole == RoleConstant.Repairer || user.RegisteredRole == RoleConstant.Inspector)
    {
      var invitations = await _invitationsRepository
        .FindAsync(x => x.Email == emailInvited, cancellationToken);

      if (invitations is null) return;

      foreach (var invitation in invitations)
      {
        var role = await _roleManager.FindByNameAsync(invitation.Role);
        if (role == null) continue;

        await _userWarehouseRoleRepository.AddAsync(new UserWarehouseRole
        {
          UserId = user.Id,
          RoleId = role.Id,
          WarehouseId = invitation.WarehouseId
        }, cancellationToken);

        await _invitationsRepository.DeleteAsync(invitation, cancellationToken);

        await _warehouseFinalUseRepository.AddAsync(
          new WarehouseFinalUses()
          {
            UserId = user.Id,
            WarehouseId = invitation.WarehouseId,
          },
          cancellationToken);
      }
    }
    else
    {
      var invitation = await _invitationsRepository
        .FirstOrDefaultAsync(x => x.Email == emailInvited && x.WarehouseId == warehouseId);

      if (invitation is null) return;

      var roleId = await _roleManager.FindByNameAsync(invitation.Role);
      if (roleId is null) return;

      await _userWarehouseRoleRepository.AddAsync(
        new UserWarehouseRole()
        {
          UserId = user.Id,
          RoleId = roleId.Id,
          WarehouseId = invitation.WarehouseId,
        }, cancellationToken);

      user.ParentId = invitation.InviteById;
      await _userManager.UpdateAsync(user);

      await _invitationsRepository.DeleteAsync(invitation, cancellationToken);

      await _warehouseFinalUseRepository.AddAsync(
        new WarehouseFinalUses()
        {
          UserId = user.Id,
          WarehouseId = invitation.WarehouseId,
        },
        cancellationToken);
    }
  }
}
