using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.FloorPlans.Create;

public class CreateFloorPlanRequestValidator : Validator<CreateFloorPlanRequest>
{
  public CreateFloorPlanRequestValidator()
  {
    RuleFor(x => x.FloorPlans)
      .NotEmpty()
      .WithMessage("FloorPlans is required.");
  }
}
