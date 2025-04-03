using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Users;
public interface IUserInspectorService
{
  Task<InspectorProfileDTO> GetInspectorProfileAsync(Guid userId);
}
