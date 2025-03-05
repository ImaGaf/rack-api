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
          .NotEmpty().WithMessage("FloorPlanId is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.Title)
          .NotEmpty().WithMessage("Title is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.Floor)
          .NotNull().WithMessage("Floor is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.TotalRack)
          .NotNull().WithMessage("TotalRack is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.TotalBay)
          .NotNull().WithMessage("TotalBay is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.Items)
          .NotEmpty().WithMessage("Items list cannot be empty.");

      // Check each element in Items
      floorPlan.RuleForEach(fp => fp.Items).ChildRules(item =>
      {
        item.RuleFor(i => i.Id)
            .NotEmpty().WithMessage("Item Id is required and must be correctly formatted.");

        item.RuleFor(i => i.Rack)
            .NotNull().WithMessage("Rack is required and must be correctly formatted.");

        item.RuleFor(i => i.Bay)
            .NotNull().WithMessage("Bay is required and must be correctly formatted.");

        item.RuleFor(i => i.Offset)
            .NotEmpty().WithMessage("Offset is required and must be correctly formatted.");

        item.RuleFor(i => i.Rotation)
            .NotNull().WithMessage("Rotation is required and must be correctly formatted.");

        // Check list Comments
        item.RuleForEach(i => i.Comments).ChildRules(comment =>
        {
          comment.RuleFor(c => c.Id)
              .NotEmpty().WithMessage("Comment Id is required and must be correctly formatted.");

          comment.RuleFor(c => c.UserName)
              .NotEmpty().WithMessage("Username is required.");

          comment.RuleFor(c => c.Comment)
              .NotEmpty().WithMessage("Comment content is required.");

          comment.RuleFor(c => c.Date)
              .NotEmpty().WithMessage("Date is required and must be correctly formatted.");
        });
      });

      // Validate `NewFiles`
      floorPlan.RuleForEach(fp => fp.NewFiles)
          .SetValidator(new InlineValidator<IFormFile>())
          .When(fp => fp.NewFiles != null && fp.NewFiles.Any(), ApplyConditionTo.CurrentValidator)
          .ChildRules(file =>
          {
              file.RuleFor(f => f.FileName)
                .Must(IsValidFileExtension)
                .WithMessage("New File must be of type PDF, PNG, JPEG, HEIC or HEIF.");

              file.RuleFor(f => f.Length)
                .LessThanOrEqualTo(10 * 1024 * 1024) // 10MB
                .WithMessage("New File size must not exceed 10MB.");
          });

      // Validate `FilesToUpdate`
      floorPlan.RuleForEach(fp => fp.FilesToUpdate)
          .SetValidator(new InlineValidator<IFormFile>())
          .When(fp => fp.FilesToUpdate != null && fp.FilesToUpdate.Any(), ApplyConditionTo.CurrentValidator)
          .ChildRules(file =>
          {
            file.RuleFor(f => f.FileName)
              .Must(IsValidFileExtension)
              .WithMessage("Update File must be of type PDF, PNG, JPEG, HEIC or HEIF.");

            file.RuleFor(f => f.Length)
              .LessThanOrEqualTo(10 * 1024 * 1024) // 10MB
              .WithMessage("Update File size must not exceed 10MB.");
          });
    });
  }

  // Method to check valid file types
  private bool IsValidFileExtension(string fileName)
  {
    var validExtensions = new[] { ".pdf", ".png", ".jpeg", ".jpg", ".heic", ".heif" };
    var fileExtension = Path.GetExtension(fileName).ToLower();

    return validExtensions.Contains(fileExtension);
  }
}
