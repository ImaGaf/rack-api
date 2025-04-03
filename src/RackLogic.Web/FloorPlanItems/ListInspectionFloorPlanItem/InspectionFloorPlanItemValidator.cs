using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.FloorPlanItems.ListInspectionFloorPlanItem;

public class InspectionFloorPlanItemValidator : Validator<InspectionFloorPlanItemRequest>
{
  public InspectionFloorPlanItemValidator()
  {
    RuleFor(x => x.WarehouseId)
      .NotEmpty().WithMessage("Required WarehouseId");
  }
}
