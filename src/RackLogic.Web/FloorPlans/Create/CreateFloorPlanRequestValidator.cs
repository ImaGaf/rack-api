using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.FloorPlans.Create;

public class CreateFloorAssetRequestValidator : Validator<CreateFloorAssetRequest>
{
  public CreateFloorAssetRequestValidator()
  {
    RuleFor(x => x.WarehouseId)
        .NotEmpty().WithMessage("WarehouseId is required.")
        .Must(BeValidGuid).WithMessage("Invalid WarehouseId format.");

    RuleFor(x => x.FloorAssets)
        .NotEmpty().WithMessage("FloorAssets is required.");

    RuleForEach(x => x.FloorAssets).ChildRules(floorPlan =>
    {
      floorPlan.RuleFor(fp => fp.Title)
          .NotEmpty().WithMessage("Title is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.Floor)
          .NotNull().WithMessage("Floor is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.TotalRack)
          .NotEmpty().WithMessage("TotalRack is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.TotalBay)
          .NotEmpty().WithMessage("TotalBay is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.Items)
          .NotEmpty().WithMessage("Items list cannot be empty.");

      floorPlan.RuleForEach(fp => fp.Items).ChildRules(item =>
      {
        item.RuleFor(i => i.Offset)
            .NotEmpty().WithMessage("Offset is required and must be correctly formatted.");

        item.RuleFor(i => i.Rotation)
            .NotNull().WithMessage("Rotation is required and must be correctly formatted.");

        item.RuleFor(i => i.BayConfigurationId)
            .NotEmpty().When(i => i.IsRequiredBayConfig == true)
            .WithMessage("BayConfigurationId for FloorPlanItems is required.");
      });

      floorPlan.RuleForEach(fp => fp.Files)
          .SetValidator(new InlineValidator<IFormFile>())
          .When(fp => fp.Files != null && fp.Files.Any(), ApplyConditionTo.CurrentValidator)
          .ChildRules(file =>
          {
            file.RuleFor(f => f.FileName) // Check file name
              .Must(IsValidFileExtension)
              .WithMessage("File must be of type PDF, PNG, JPEG, HEIC or HEIF.");

            file.RuleFor(f => f.Length)
              .LessThanOrEqualTo(10 * 1024 * 1024) // 10MB
              .WithMessage("File size must not exceed 10MB.");
          });
      });
    }

  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }

  private bool IsValidFileExtension(string fileName)
  {
    var validExtensions = new[] { ".pdf", ".png", ".jpeg", ".jpg", ".heic", ".heif" };
    var fileExtension = Path.GetExtension(fileName).ToLower();

    return validExtensions.Contains(fileExtension);
  }
}
