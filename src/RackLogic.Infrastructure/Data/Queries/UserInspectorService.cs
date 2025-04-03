using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.Users;

namespace RackLogic.Infrastructure.Data.Queries;
public class UserInspectorService : IUserInspectorService
{
  private readonly AppDbContext _context;
  public UserInspectorService(AppDbContext context)
  {
    _context = context;
  }
  public async Task<InspectorProfileDTO> GetInspectorProfileAsync(Guid userId)
  {
    var inspectorProfile = await _context.InspectorProfiles
        .Where(ip => ip.UserId == userId)
        .FirstOrDefaultAsync();

    return inspectorProfile!.ToInspectorProfileDTO();
  }
}
