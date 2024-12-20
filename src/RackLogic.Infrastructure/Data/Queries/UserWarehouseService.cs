using System.Data;
using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;
public class UserWarehouseService : IUserWarehouseService
{
  private readonly AppDbContext _context;

  public UserWarehouseService(AppDbContext context)
  {
    _context = context;
  }
  public async Task<(List<ManageUserDTO> Users, int TotalRecords)> GetUsersByWarehouseIdAsync(Guid warehouseId, int pageNumber, int pageSize)
  {
    var queryListUsers = (from userWarehouseRole in _context.UserWarehouseRoles
                          where userWarehouseRole.WarehouseId == warehouseId
                          join user in _context.Users on userWarehouseRole.UserId equals user.Id
                          join role in _context.Roles on userWarehouseRole.RoleId equals role.Id
                          select new ManageUserDTO
                          {
                            UserID = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Role = role.Name
                          });

    int totalRecords = await queryListUsers.CountAsync();

    var users = await queryListUsers
        .OrderBy(u => u.FirstName)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (users, totalRecords);
  }
}
