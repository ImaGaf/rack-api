using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.Users.GetInspectorProfile;
public class GetInspectorProfileHandler(
  IRepositoryWithExpression<InspectorProfile> _inspectorProfileRepository,
  IUserInspectorService _userInspectorService)
  : IQueryHandler<GetInspectorProfileQuery, Result<InspectorProfileDTO>>
{
  public async Task<Result<InspectorProfileDTO>> Handle(GetInspectorProfileQuery request, CancellationToken cancellationToken)
  {
    var creatorId = request.UserCreatedId;
    var existingInspectorProfile = await _inspectorProfileRepository.FirstOrDefaultAsync(i => i.UserId == creatorId, cancellationToken);
    if (existingInspectorProfile == null) return Result.Invalid(new ValidationError("User does not have inspector profile."));

    var inspectorProfile = await _userInspectorService.GetInspectorProfileAsync(creatorId);
    return Result.Success(inspectorProfile);
  }
}
