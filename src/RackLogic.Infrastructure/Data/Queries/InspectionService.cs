using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Inspections.SearchReport;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;

public class InspectionService(AppDbContext dbContext) : IInspectionService
{
  public async Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    var inspection = Guard.Against.Null(await dbContext.Inspections
      .Where(i => i.Id == id)
      .Include(i => i.Warehouse)
      .Include(i => i.AuthorisePerson)
      .FirstOrDefaultAsync(cancellationToken));

    var result = inspection!.ToDto();

    result.AuthorisedPersonName = inspection.AuthorisePerson?.FirstName + " " + inspection.AuthorisePerson?.LastName;

    return result;
  }

  public async Task<PagedDataDTO<InspectionReportDto>> SearchReportAsync(SearchInspectionReportQuery request, Guid userId, CancellationToken cancellationToken)
  {
    var userWarehouseIds = await dbContext.UserWarehouseRoles
      .Where(uwr => uwr.UserId == userId)
      .Select(uwr => uwr.WarehouseId)
      .ToListAsync(cancellationToken);

    var query = dbContext.Inspections
      .Include(i => i.Warehouse)
      .Include(i => i.AuthorisePerson)
      .Where(i => i.IsConfirmAuthorization == true && userWarehouseIds.Contains(i.WarehouseId!.Value));

    if (request.Search != null)
    {
      var search = request.Search;
      query = query.Where(i => i.Warehouse!.CompanyName!.Contains(search) ||
                               (i.AuthorisePerson!.FirstName + " " + i.AuthorisePerson.LastName).Contains(search));
    }

    var totalItem = await query.CountAsync(cancellationToken);

    var result = await query
      .Skip((request.PageNumber - 1) * request.PageSize)
      .Take(request.PageSize)
      .Select(i => i.ToReportDtoWithFullName())
      .ToListAsync(cancellationToken);

    return new PagedDataDTO<InspectionReportDto>
    {
      TotalRecords = totalItem,
      PageNumber = request.PageNumber,
      PageSize = request.PageSize,
      DataDTO = result
    };
  }
}
