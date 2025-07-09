using Ardalis.Result;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Warehouses;

public interface IInvitationService
{
  Task AcceptInvitationAsync(string emailInvited, Guid warehouseId, ApplicationUser user, CancellationToken cancellationToken = default);

  Task<Result<bool>> ProcessInvitationAsync(string email, Guid warehouseId, string inviteRole, CancellationToken ct);
}
