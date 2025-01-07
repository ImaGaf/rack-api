using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.FloorPlans.Update;

public class UpdateFloorPlanRequestValidator : Validator<UpdateFloorPlanRequest>
{
  public UpdateFloorPlanRequestValidator()
  {
    RuleFor(x => x.FloorPlans)
      .NotEmpty()
      .WithMessage("FloorPlans is required.");
  }
}
