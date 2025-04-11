using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.DefectLogs.Update;

public class UpdateDefectLogValidator : Validator<UpdateDefectLogRequest>
{
  public UpdateDefectLogValidator()
  {
    RuleFor(x => x.Id)
      .NotNull()
      .WithMessage("Defect Log Id is required.");

    RuleFor(x => x.Comment)
      .MaximumLength(2000);
  }
}
