using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.BayConfigurator.Create;

public class CreateBayConfiguratorRequestValidator : Validator<CreateBayConfiguratorRequest>
{
  public CreateBayConfiguratorRequestValidator()
  {
    RuleFor(x => x.WarehouseId)
        .NotEmpty().WithMessage("BrandId is required.")
        .Must(BeValidGuid).WithMessage("Invalid BrandId format.");
    RuleFor(x => x.BayConfigurator).NotNull().WithMessage("BayConfigurator is required.");

    RuleFor(x => x.BayConfigurator.BayConfigId)
        .NotEmpty().WithMessage("BayConfigId is required.");

    RuleFor(x => x.BayConfigurator.RackingType)
        .NotEmpty().WithMessage("RackingType is required.");

    RuleFor(x => x.BayConfigurator.AsComplianceVersion)
        .NotEmpty().WithMessage("AsComplianceVersion is required.");

    RuleFor(x => x.BayConfigurator.Supplier)
        .NotEmpty().WithMessage("Supplier is required.");

    RuleFor(x => x.BayConfigurator.StructuralDesigner)
        .NotEmpty().WithMessage("StructuralDesigner is required.");

    RuleFor(x => x.BayConfigurator.ProjectRef)
        .NotEmpty().WithMessage("ProjectRef is required.");

    RuleFor(x => x.BayConfigurator.ToleranceGrade)
        .GreaterThanOrEqualTo(0).WithMessage("ToleranceGrade must be >= 0.");

    RuleFor(x => x.BayConfigurator.TotalBayLoad)
        .GreaterThan(0).WithMessage("TotalBayLoad must be greater than 0.");

    RuleFor(x => x.BayConfigurator.UpRight)
        .NotNull().WithMessage("UpRight is required.");

    RuleFor(x => x.BayConfigurator.Beam)
        .NotNull().WithMessage("Beam is required.");

    RuleFor(x => x.BayConfigurator.BeamLevels)
        .NotEmpty().WithMessage("BeamLevels must not be empty.");

    RuleFor(x => x.BayConfigurator.BracingHorizontal)
        .NotNull().WithMessage("BracingHorizontal is required.");

    RuleFor(x => x.BayConfigurator.BracingHorizontalLevels)
        .NotEmpty().WithMessage("BracingHorizontalLevels must not be empty.");

    RuleFor(x => x.BayConfigurator.BracingDiagonal)
        .NotNull().WithMessage("BracingDiagonal is required.");

    RuleFor(x => x.BayConfigurator.BracingDiagonalLevels)
        .NotEmpty().WithMessage("BracingDiagonalLevels must not be empty.");

    RuleFor(x => x.BayConfigurator.RowSpacer)
        .NotNull().WithMessage("RowSpacer is required.");

    RuleFor(x => x.BayConfigurator.RowSpacerLevels)
        .NotEmpty().WithMessage("RowSpacerLevels must not be empty.");

    RuleFor(x => x.BayConfigurator.InstallationDate)
      .NotNull()
      .When(x => !x.BayConfigurator.IsEstimatedInstallDate)
      .WithMessage("InstallationDate is required when IsEstimatedInstallDate is false.");
  }
  private bool BeValidGuid(Guid guid)
  {
    return guid != Guid.Empty; // Ensure it's not an empty GUID
  }
}
