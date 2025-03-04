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

    RuleFor(x => x.FloorAsset)
        .NotEmpty().WithMessage("FloorAssets is required.");

    RuleForEach(x => x.FloorAsset).ChildRules(floorPlan =>
    {
      floorPlan.RuleFor(fp => fp.Title)
          .NotEmpty().WithMessage("Title is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.Floor)
          .NotEmpty().WithMessage("Floor is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.TotalRack)
          .NotEmpty().WithMessage("TotalRack is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.TotalBay)
          .NotEmpty().WithMessage("TotalBay is required and must be correctly formatted.");

      floorPlan.RuleFor(fp => fp.Items)
          .NotEmpty().WithMessage("Items list cannot be empty.");

      floorPlan.RuleForEach(fp => fp.Items).ChildRules(item =>
      {
        item.RuleFor(i => i.Rack)
            .NotEmpty().WithMessage("Rack is required and must be correctly formatted.");

        item.RuleFor(i => i.Bay)
            .NotEmpty().WithMessage("Bay is required and must be correctly formatted.");

        item.RuleFor(i => i.Offset)
            .NotEmpty().WithMessage("Offset is required and must be correctly formatted.");

        item.RuleFor(i => i.Rotation)
            .NotEmpty().WithMessage("Rotation is required and must be correctly formatted.");
      });

      floorPlan.RuleForEach(fp => fp.Files)
      .SetValidator(new InlineValidator<IFormFile>())
      .When(fp => fp.Files != null && fp.Files.Any(), ApplyConditionTo.CurrentValidator)
      .ChildRules(file =>
      {
        file.RuleFor(f => f.ContentType)
      .Must(IsValidFileType)
      .WithMessage("File must be of type PDF, PNG, or JPEG.");

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

  private bool IsValidFileType(string? contentType)
  {
    return contentType == "application/pdf" ||   // PDF
           contentType == "image/png" ||        // PNG
           contentType == "image/jpeg";         // JPEG
  }
}
