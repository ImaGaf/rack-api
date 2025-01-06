using System.Data;
using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Brands;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;
public class UserWarehouseService : IUserWarehouseService
{
  private readonly AppDbContext _context;

  public UserWarehouseService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<OEMBrandDTO>> GetAllBrandsAsync()
  {
    var query = from brand in _context.OEMBrands
                orderby brand.Name
                select new OEMBrandDTO
                {
                  Id = brand.Id,
                  Name = brand.Name,
                  Path = brand.Path
                };

    var brands = await query.ToListAsync();
    return brands;

  }

  public async Task<(List<ListComponentBrandDTO> Products, int TotalRecords)> GetProductsByBrandAsync(Guid brandId, int pageNumber, int pageSize)
  {
    var queryListProducts = from product in _context.Products
                            where product.ManufacturerId == brandId
                            select new ListComponentBrandDTO
                            {
                              Id = product.Id,
                              Category = product.Category,
                              Height = product.Height,
                              Width = product.Width,
                              Length = product.Length,
                              Code = product.Code,
                              Description = product.Description,
                              Orientation = product.Orientation,
                              DistanceCenterToCenter = product.DistanceCenterToCenter,
                            };

    int totalRecords = await queryListProducts.CountAsync();
    var pageSizeProduct = pageSize == 0? totalRecords : pageSize;

    var products = await queryListProducts
                .OrderBy(p => p.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSizeProduct)
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
                  DistanceCenterToCenter = product.DistanceCenterToCenter
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
}
