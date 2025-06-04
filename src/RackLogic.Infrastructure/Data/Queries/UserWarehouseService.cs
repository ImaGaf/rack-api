using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.Common.Constants;
using RackLogic.UseCases.Brands;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses;
using RackLogic.UseCases.Warehouses.SearchWarehouse;
using RackLogic.UseCases.Warehouses.SearchWarehouseCertificationInspection;

namespace RackLogic.Infrastructure.Data.Queries;
public class UserWarehouseService(
  AppDbContext _context,
  RoleManager<IdentityRole<Guid>> _roleManager,
  IUser _user)
  : IUserWarehouseService
{

  public async Task<List<GetAllBrandDTO>> GetAllBrandAsync()
  {
    var result = await _context.OEMBrands
        .GroupJoin(
            _context.Products,
            brand => brand.Id,
            product => product.ManufacturerId,
            (brand, products) => brand.ToGetAllBrandDTO(products.ToList())
        ).ToListAsync();

    if (result is null) return new List<GetAllBrandDTO>();

    return result;
  }

  public async Task<List<OEMBrandDTO>> GetAllBrandsAsync()
  {
    var brands = await _context.OEMBrands
        .Where(x => x.WarehouseId == null)
        .OrderBy(brand => brand.Name)
        .ToListAsync();

    return brands.Select(brand => brand.ToOEMBrandDTO()).ToList();
  }

  public async Task<(List<ProductDTO> Products, int TotalRecords)> GetProductsByBrandAsync(Guid? brandId, Guid? warehouseId, int pageNumber, int pageSize, bool isGetAll)
  {
    var baseQuery = _context.Products
        .Where(x =>
          (brandId == null || x.ManufacturerId == brandId) &&
          (x.WarehouseId == null || x.WarehouseId == warehouseId));

    int totalRecords = await baseQuery.CountAsync();

    if (!isGetAll)
    {
      baseQuery = baseQuery
        .OrderBy(p => p.Category)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize);
    }

    var products = await baseQuery
        .Select(x => x.ToProductDto())
        .ToListAsync();

    return (products, totalRecords);
  }

  public async Task<List<ProductDTO>> GetProductsByCategoryAndManufacturerAsync(Guid manufacturerId, string category)
  {
    var query = from product in _context.Products
                where product.ManufacturerId == manufacturerId && product.Category == category
                select new ProductDTO
                {
                  Id = product.Id,
                  Height = product.Height,
                  Width = product.Width,
                  Length = product.Length,
                  Code = product.Code,
                  Description = product.Description,
                  Orientation = product.Orientation,
                  DistanceCenterToCenter = product.DistanceCenterToCenter,
                  ComponentType = product.ComponentType
                };

    var products = await query.ToListAsync();
    return products;
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

  public async Task<WarehousesDTO?> GetLastWarehouseForUserAsync(Guid userId)
  {
    var warehouseFinalUse = await _context.WarehouseFinalUses
    .FirstOrDefaultAsync(wf => wf.UserId == userId);

    if (warehouseFinalUse == null) return null;

    // Query warehouse information based on WarehouseId from WarehouseFinalUse
    var warehouse = await _context.Warehouses
        .Where(w => w.Id == warehouseFinalUse.WarehouseId)
        .Select(w => new WarehousesDTO
        {
          WarehouseId = w.Id,
          Name = w.CompanyName,
          FilePathImage = w.FileImageUrl
        })
        .FirstOrDefaultAsync();

    return warehouse;
  }
  public async Task<List<WarehousesDTO>> GetWarehousesByUserIdAsync(Guid userId)
  {
    var query = from userWarehouseRole in _context.UserWarehouseRoles
                where userWarehouseRole.UserId == userId
                join warehouse in _context.Warehouses
                on userWarehouseRole.WarehouseId equals warehouse.Id
                select new WarehousesDTO
                {
                  WarehouseId = warehouse.Id,
                  Name = warehouse.CompanyName,
                  FilePathImage = warehouse.FileImageUrl
                };
    return await query.ToListAsync();
  }

  public async Task<bool> CheckPermissionUserRequestAsync(Guid userId, Guid warehouseId)
  {
    var result = await _context.UserWarehouseRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises &&
      role.Name != RoleConstant.ClientManager &&
      role.Name != RoleConstant.Inspector) return false;

    return true;
  }

  public async Task<WarehouseProfileDTO?> GetWarehouseByIdAsync(Guid warehouseId)
  {
    return await _context.Warehouses
                  .Where(w => w.Id == warehouseId)
                  .Select(w => w.ToResponse())
                  .FirstOrDefaultAsync();
  }

  public async Task<OEMBrandDTO> GetBrandByComponentAsync(Guid componentId)
  {
    var component = await _context.Components
      .Include(c => c.BayConfiguration!)
      .ThenInclude(b => b.Manufacturer)
      .FirstOrDefaultAsync(c => c.Id == componentId);

    var brand = component!.BayConfiguration!.Manufacturer;

    if (brand == null)
    {
      return new OEMBrandDTO();
    }

    return new OEMBrandDTO { Id = brand.Id, Name = brand.Name, Path = brand.Path, Code = brand.Code };
  }

  public async Task<PagedDataDTO<WarehouseDto>> SearchWarehouseByUserIdAsync(SearchWarehouseQuery request, Guid? userId, CancellationToken cancellationToken)
  {
    var query = from userWarehouseRole in _context.UserWarehouseRoles
                where userWarehouseRole.UserId == userId
                join warehouse in _context.Warehouses
                  on userWarehouseRole.WarehouseId equals warehouse.Id
                select warehouse;

    if (request.Search != null)
    {
      var search = request.Search;
      query = query.Where(w => w.CompanyName.Contains(search) ||
                               w.City!.Contains(search) ||
                               w.State!.Contains(search) ||
                               w.ClientPhoneNumber.Contains(search) ||
                               (w.OwnerFirstName + " " + w.OwnerLastName).Contains(search));
    }

    var totalItem = await query.CountAsync(cancellationToken);

    var result = await query
      .OrderBy(x => x.CompanyName)
      .Skip((request.PageNumber - 1) * request.PageSize)
      .Take(request.PageSize)
      .Select(x => x.ToDto())
      .ToListAsync(cancellationToken);

    return new PagedDataDTO<WarehouseDto>
    {
      TotalRecords = totalItem,
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      DataDTO = result
    };
  }

  public async Task<PagedDataDTO<WarehouseCertificationInspectionDto>> SearchWarehouseCertificationInspectionAsync(
    SearchWarehouseCertificationInspectionQuery request, 
    Guid? userId, 
    CancellationToken cancellationToken)
  {
    var query = from userWarehouseRole in _context.UserWarehouseRoles 
      where userWarehouseRole.UserId == userId
      join role in _context.Roles
        on userWarehouseRole.RoleId equals role.Id
      where role.Name == RoleConstant.Inspector
      join warehouse in _context.Warehouses
        on userWarehouseRole.WarehouseId equals warehouse.Id
      select warehouse;

    if (request.Search != null)
    {
      var search = request.Search;
      query = query.Where(w => w.CompanyName.Contains(search) ||
                               w.City!.Contains(search));
    }

    var totalItem = await query.CountAsync(cancellationToken);

    var result = await query
      .OrderBy(w => w.InspectionDueDate)
      .Skip((request.PageNumber - 1) * request.PageSize)
      .Take(request.PageSize)
      .Select(w => w.ToWarehouseCertificationInspectionDto())
      .ToListAsync(cancellationToken);

    return new PagedDataDTO<WarehouseCertificationInspectionDto>
    {
      TotalRecords = totalItem,
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      DataDTO = result
    };
  }

  public bool HasPermission(string entityCode, string privilegeCode)
  {
    if (_user.Permissions == null)
      return false;

    var requiredPermissions = new[]
    {
      $"{entityCode}.full",
      $"{entityCode}.full-themselves",
      $"{entityCode}.full-everyone",
      $"{privilegeCode}"
    };

    return _user.Permissions.Any(p => requiredPermissions.Contains(p));
  }
}
