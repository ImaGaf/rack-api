using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.BayConfigurator.UpdateSWL;
public class UpdateSWLHandler(
  IRepositoryWithExpression<SWLSignage> _sWLSignageRepository,
  IRepositoryWithExpression<BayConfiguration> _bayConfigurationRepository)
  : ICommandHandler<UpdateSWLCommand, Result<BayConfigurationDTO>>
{
  public async Task<Result<BayConfigurationDTO>> Handle(UpdateSWLCommand request, CancellationToken cancellationToken)
  {
    var sWLSignage = await _sWLSignageRepository.FirstOrDefaultAsync(x => x.BayConfigurationId == request.Id, cancellationToken);

    if (sWLSignage is null) 
    {
      await _sWLSignageRepository.AddAsync(request.ToEntity(), cancellationToken);
    }
    else
    {
      await _sWLSignageRepository.UpdateAsync(sWLSignage.Mapping(request));
    }

    var bayConfig = await _bayConfigurationRepository.FirstOrDefaultAsync(
      x => x.Id == request.Id,
      includes:[
        i => i.SWLSignage,
        i => i.Components,
        i => i.BayConfigurationLevels,
        i => i.Manufacturer]);

    return bayConfig!.Mapping();
  }
}
