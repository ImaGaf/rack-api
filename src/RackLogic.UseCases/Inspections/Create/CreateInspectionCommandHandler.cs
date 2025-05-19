using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Interfaces;
using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections.Create;

public class CreateInspectionCommandHandler(
  IRepositoryWithExpression<Inspection> _inspectionRepository,
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IRepositoryWithExpression<FloorPlan> _floorPlanRepository,
  IUser _user
  ) : ICommandHandler<CreateInspectionCommand, Result<CreateInspectionDto>>
{
  public async Task<Result<CreateInspectionDto>> Handle(CreateInspectionCommand request, CancellationToken cancellationToken)
  {
    var warehouse = await _warehouseRepository.FirstOrDefaultAsync(w => w.Id == request.WarehouseId, cancellationToken);

    if (warehouse == null)
      return Result.NotFound("WarehouseId is not found");

    var entity = request.ToEntity();

    entity.IsConfirmAuthorization = false;
    entity.InspectionDate = DateTime.UtcNow;
    entity.AuthorisePersonId = _user.Id;

    var floorPlan = await _floorPlanRepository.FirstOrDefaultAsync(f => f.Id == request.FloorPlanId, cancellationToken, f => f.Inspections);

    if (floorPlan is null)
      return Result.NotFound("FloorPlanId is not found");

    var continueInspection = floorPlan.Inspections?.FirstOrDefault(x => x.InspectionDate.Date <= DateTime.Now.Date && !x.IsConfirmAuthorization);

    if (continueInspection is null)
    {
      if (request.NextInspectionDate == null)
      {
        entity.NextInspectionDate = DateTime.UtcNow.AddYears(1);
        warehouse.InspectionDueDate = entity.NextInspectionDate;
      }
      else
      {
        warehouse.InspectionDueDate = request.NextInspectionDate;
      }

      await _inspectionRepository.AddAsync(entity, cancellationToken);

      await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);
    }

    var result = new CreateInspectionDto
    {
      Id = continueInspection is null ? entity.Id : continueInspection.Id,
      IsNewInspection = continueInspection is null
    };

    return Result.Success(result);
  }
}
