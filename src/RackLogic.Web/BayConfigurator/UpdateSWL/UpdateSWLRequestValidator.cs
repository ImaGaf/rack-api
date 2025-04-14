using FastEndpoints;
using FluentValidation;

namespace RackLogic.Web.BayConfigurator.UpdateSWL;

public class UpdateSWLRequestValidator : Validator<UpdateSWLRequest>
{
  public UpdateSWLRequestValidator()
  {
    RuleFor(x => x.BayConfigurationId)
      .NotNull()
      .WithMessage("SWL Id is required.");
  }
}
