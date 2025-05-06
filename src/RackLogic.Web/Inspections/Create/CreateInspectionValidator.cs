using FastEndpoints;
using FluentValidation;
using RackLogic.UseCases.Inspections.Create;

namespace RackLogic.Web.Inspections.Create;

public class CreateInspectionValidator : Validator<CreateInspectionCommand>
{
  public CreateInspectionValidator()
  {
    RuleFor(x => x.FloorPlanId)
      .NotNull()
      .WithMessage("FloorPlanId is required.");

    RuleFor(x => x.WarehouseId)
      .NotNull()
      .WithMessage("WarehouseId is required.");
  }
}
