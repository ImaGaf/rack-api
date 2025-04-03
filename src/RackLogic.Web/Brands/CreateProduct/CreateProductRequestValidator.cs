using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.Brands.CreateProduct;

public class CreateProductRequestValidator : Validator<CreateProductRequest>
{
  public CreateProductRequestValidator()
  {
    RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .Must(category => new[] { "UPRIGHT", "BEAM", "BRACE", "ROWSPACER" }.Contains(category.ToUpper()))
            .WithMessage("Invalid category. Must be UPRIGHT, BEAM, BRACE, or ROWSPACER.");

    RuleFor(x => x.Code)
        .NotEmpty().WithMessage("Code is required.");

    RuleFor(x => x.Description)
        .NotEmpty().WithMessage("Description is required.");

    // Category-specific validation
    When(x => x.Category?.ToUpper() == "UPRIGHT", () =>
    {
      RuleFor(x => x.Height).NotNull().WithMessage("Height is required for UPRIGHT.");
      RuleFor(x => x.Width).NotNull().WithMessage("Width is required for UPRIGHT.");
      RuleFor(x => x.MaterialThickness).NotNull().WithMessage("Material Thickness is required for UPRIGHT.");
    });

    When(x => x.Category?.ToUpper() == "BEAM", () =>
    {
      RuleFor(x => x.BeamStyle).NotEmpty().WithMessage("Beam Style is required for BEAM.");
      RuleFor(x => x.ClearEntryWidth).NotNull().WithMessage("Clear Entry Width is required for BEAM.");
      RuleFor(x => x.Height).NotNull().WithMessage("Height is required for BEAM.");
      RuleFor(x => x.Width).NotNull().WithMessage("Width is required for BEAM.");
      RuleFor(x => x.ConnectorType).NotEmpty().WithMessage("Connector Type is required for BEAM.");
      RuleFor(x => x.FinishType).NotEmpty().WithMessage("Finish Type is required for BEAM.");
    });

    When(x => x.Category?.ToUpper() == "BRACE", () =>
    {
      RuleFor(x => x.Orientation).NotEmpty().WithMessage("Orientation is required for BRACE.");
      RuleFor(x => x.DepthOfFrame).NotNull().WithMessage("Depth of Frame is required for BRACE.");
      RuleFor(x => x.DistanceCenterToCenter).NotNull().WithMessage("Distance Center-to-Center is required for BRACE.");
      RuleFor(x => x.Height).NotNull().WithMessage("Height is required for BRACE.");
      RuleFor(x => x.FinishType).NotEmpty().WithMessage("Finish Type is required for BRACE.");
    });

    When(x => x.Category?.ToUpper() == "ROWSPACER", () =>
    {
      RuleFor(x => x.ClearanceBetweenFrames).NotNull().WithMessage("Clearance Between Frames is required for ROWSPACER.");
      RuleFor(x => x.FinishType).NotEmpty().WithMessage("Finish Type is required for ROWSPACER.");
    });
  }
}
