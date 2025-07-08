using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Users.GetInspectorProfile;
public class GetInspectorProfileHandler(
  IRepositoryWithExpression<ApplicationUser> _userRepository,
  IRepositoryWithExpression<InspectorProfile> _inspectorProfileRepository,
  IUserInspectorService _userInspectorService)
  : IQueryHandler<GetInspectorProfileQuery, Result<InspectorProfileDTO>>
{
  public async Task<Result<InspectorProfileDTO>> Handle(GetInspectorProfileQuery request, CancellationToken cancellationToken)
  {
    var creatorId = request.UserCreatedId;
    var existingInspectorProfile = await _inspectorProfileRepository.FirstOrDefaultAsync(i => i.UserId == creatorId, cancellationToken);
    if (existingInspectorProfile == null) 
    {
      var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == creatorId);
      return user!.UserToInspectorDTO();
    }

    var inspectorProfile = await _userInspectorService.GetInspectorProfileAsync(creatorId);
    return Result.Success(inspectorProfile);
  }
}
