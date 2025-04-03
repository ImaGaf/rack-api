using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Users.GetInspectorProfile;
public record GetInspectorProfileQuery(Guid UserCreatedId) : IQuery<Result<InspectorProfileDTO>>;
