using Microsoft.EntityFrameworkCore;
using RackLogic.UseCases.Inspections;

namespace RackLogic.Infrastructure.Data.Queries;

public class InspectionService(AppDbContext dbContext) : IInspectionService
{
  public async Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return await dbContext.Inspections
      .Where(d => d.Id == id)
      .Select(d => new InspectionDto
      {
        Id = d.Id,
        WarehouseId = d.WarehouseId,
        FloorPlanId = d.FloorPlanId,
        AuthorisedPersonId = d.AuthorisePersonId,
        InspectionDate = d.InspectionDate,
        NextInspectionDate = d.NextInspectionDate,
        AuthorisedPersonName = d.AuthorisePerson!.FirstName + " " + d.AuthorisePerson.LastName
      })
      .FirstOrDefaultAsync(cancellationToken);
  }
}
