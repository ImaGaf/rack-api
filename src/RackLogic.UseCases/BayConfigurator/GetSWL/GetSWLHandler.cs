using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.BayConfigurator.GetSWL;
public class GetSWLHandler(
  IRepositoryWithExpression<BayConfiguration> _bayConfigurationRepositoy)
  : ICommandHandler<GetSWLCommand, Result<SWLSignageDTO>>
{
  public async Task<Result<SWLSignageDTO>> Handle(GetSWLCommand request, CancellationToken cancellationToken)
  {
    var bayConfiguration = await _bayConfigurationRepositoy.FirstOrDefaultAsync(
      x => x.Id == request.BayConfigurationId,
      includes:
        [i => i.SWLSignage,
        i => i.Manufacturer],
      cancellationToken: cancellationToken);

    if (bayConfiguration is null ) Result.Invalid(new ValidationError("SWL Id not found."));

    return bayConfiguration.ToDto();
  }
}
