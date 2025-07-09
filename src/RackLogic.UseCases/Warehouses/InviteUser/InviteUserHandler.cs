using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Constants;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Warehouses.InviteUser;

public class InviteUserHandler(
  IUserWarehouseService _userWarehouseService,
  IInvitationService _invitationService,
  IUser _user)
  : ICommandHandler<InviteUserCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(InviteUserCommand request, CancellationToken cancellationToken)
  {
    if (_user.Id is null) return Result.Unauthorized();

    if (!_userWarehouseService.HasPermission(PermissionConstant.ManageUser.Code, PermissionConstant.ManageUser.Create))
      return Result.Forbidden(ErrorMessages.ManageUser.ErrorMessageCreate);

    return await _invitationService.ProcessInvitationAsync(
      request.Email,
      request.WarehouseId,
      request.Role,
      cancellationToken);
  }
}
