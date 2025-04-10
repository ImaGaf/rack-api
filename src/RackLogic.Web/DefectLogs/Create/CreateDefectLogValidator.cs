using FastEndpoints;
using FluentValidation;
using RackLogic.UseCases.DefectLogs.Create;

namespace RackLogic.Web.DefectLogs.Create;

public class CreateDefectLogValidator : Validator<CreateDefectLogCommand>
{
  public CreateDefectLogValidator()
  {
    RuleFor(x => x.DefectType)
      .NotNull()
      .WithMessage("Defect Type is required.");

    RuleFor(x => x.DefectGrade)
      .NotNull()
      .WithMessage("Defect Grade is required.");

    RuleFor(x => x.DefectPriority)
      .NotNull()
      .WithMessage("Defect Priority is required.");

    RuleFor(x => x.CorrectiveAction)
      .NotNull()
      .WithMessage("Corrective Action is required.");

    RuleFor(x => x.ComponentIndex)
      .NotNull()
      .WithMessage("Component Index is required.");

    RuleFor(x => x.DefectStatus)
      .NotNull()
      .WithMessage("Defect status is required.");

    RuleFor(x => x.ComponentId)
      .NotEmpty()
      .WithMessage("ComponentId is required.");

    RuleFor(x => x.FloorPlanItemId)
      .NotEmpty()
      .WithMessage("FloorPlanItemId is required.");

    RuleFor(x => x.Comment)
      .MaximumLength(2000);
  }
}
