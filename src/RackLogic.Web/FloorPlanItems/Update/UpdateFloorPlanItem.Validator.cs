using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.FloorPlanItems.Update;

public class UpdateFloorPlanItemValidator : Validator<UpdateFloorPlanItemRequest>
{
  public UpdateFloorPlanItemValidator()
  {
    RuleFor(i => i.BayId)
      .NotEmpty()
      .WithMessage("BayId is required and must be correctly formatted.")
      .MaximumLength(30)
      .WithMessage("BayId's maximum length is 30 characters");
  }
}
