using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.FloorPlanItems.Update;
public class UpdateFloorPlanItemHandler(
  IRepositoryWithExpression<FloorPlanItem> repository)
  : ICommandHandler<UpdateFloorPlanItemCommand, Result<FloorPlanItemDto>>
{
  public async Task<Result<FloorPlanItemDto>> Handle(
    UpdateFloorPlanItemCommand request,
    CancellationToken cancellationToken)
  {
    var item = await repository.FirstOrDefaultAsync(x => x.Id == request.Id);

    if (item == null)
    {
      return Result.Invalid(new ValidationError("FloorPlanItem is not existed"));
    }

    item.BayId = request.BayId;
    await repository.UpdateAsync(item);
    return item.Mapping();
  }
}
