using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.FloorPlans.Update;

public class UpdateFloorPlanRequestValidator : Validator<UpdateFloorPlanRequest>
{
  public UpdateFloorPlanRequestValidator()
  {
    RuleFor(x => x.WarehouseId)
        .NotEmpty().WithMessage("WarehouseId is required.");


    RuleFor(x => x.FloorAsset)
        .NotEmpty().WithMessage("FloorAsset is required.");

    // Check each element in FloorAsset
    RuleForEach(x => x.FloorAsset).ChildRules(floorPlan =>
    {
      floorPlan.RuleFor(fp => fp.FloorPlanId)
          .NotEmpty().WithMessage("FloorPlanId is required.");

      floorPlan.RuleFor(fp => fp.Title)
          .NotEmpty().WithMessage("Title is required.");

      floorPlan.RuleFor(fp => fp.Floor)
          .NotEmpty().WithMessage("Floor is required.");

      floorPlan.RuleFor(fp => fp.TotalRack)
          .NotEmpty().WithMessage("TotalRack is required.");

      floorPlan.RuleFor(fp => fp.TotalBay)
          .NotEmpty().WithMessage("TotalBay is required.");

      //floorPlan.RuleFor(fp => fp.Items)
      //    .NotEmpty().WithMessage("Items list cannot be empty.");

     // Check each element in Items
      floorPlan.RuleForEach(fp => fp.Items).ChildRules(item =>
      {
        item.RuleFor(i => i.Id)
            .NotEmpty().WithMessage("Item Id is required.");

        item.RuleFor(i => i.Rack)
            .NotEmpty().WithMessage("Rack is required.");

        item.RuleFor(i => i.Bay)
            .NotEmpty().WithMessage("Bay is required.");

        item.RuleFor(i => i.Offset)
            .NotEmpty().WithMessage("Offset is required.");

        item.RuleFor(i => i.Rotation)
            .NotEmpty().WithMessage("Rotation is required.");

        // Check list Comments
        item.RuleForEach(i => i.Comments).ChildRules(comment =>
        {
          comment.RuleFor(c => c.Id)
              .NotEmpty().WithMessage("Comment Id is required.");

          comment.RuleFor(c => c.UserName)
              .NotEmpty().WithMessage("Username is required.");

          comment.RuleFor(c => c.Comment)
              .NotEmpty().WithMessage("Comment content is required.");

          comment.RuleFor(c => c.Date)
              .NotEmpty().WithMessage("Date is required.");
        });
      });
    });
  }
}
