using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Interfaces;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections.Create;

public class CreateInspectionCommandHandler(
  IRepositoryWithExpression<Inspection> inspectionRepository,
  IRepositoryWithExpression<Warehouse> warehouseRepository,
  IUser user
  ) : ICommandHandler<CreateInspectionCommand, Result<InspectionDto>>
{
  public async Task<Result<InspectionDto>> Handle(CreateInspectionCommand request, CancellationToken cancellationToken)
  {
    var warehouse = await warehouseRepository.FirstOrDefaultAsync(w => w.Id == request.WarehouseId, cancellationToken);

    if (warehouse == null)
    {
      return Result.Error("Warehouse not found");
    }

    var entity = request.ToEntity();

    entity.IsConfirmAuthorization = false;
    entity.InspectionDate = DateTime.UtcNow;
    entity.AuthorisePersonId = user.Id;

    if (request.NextInspectionDate == null)
    {
      entity.NextInspectionDate = DateTime.UtcNow.AddYears(1);
      warehouse.InspectionDueDate = entity.NextInspectionDate;
    } 
    else 
    {
      warehouse.InspectionDueDate = request.NextInspectionDate;
    }

    await inspectionRepository.AddAsync(entity, cancellationToken);

    await warehouseRepository.UpdateAsync(warehouse, cancellationToken);

    var result = entity.ToDto();

    result.Warehouse = warehouse.ToDto();

    return Result.Success(result);
  }
}
