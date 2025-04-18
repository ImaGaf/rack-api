using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.BayConfigurator.UpdateSWL;
public class UpdateSWLHandler(
  IRepositoryWithExpression<SWLSignage> _sWLSignageRepository)
  : ICommandHandler<UpdateSWLCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(UpdateSWLCommand request, CancellationToken cancellationToken)
  {
    var sWLSignage = await _sWLSignageRepository.FirstOrDefaultAsync(x => x.BayConfigurationId == request.Id, cancellationToken);

    if (sWLSignage is null) return await CreateSWLSignageAsync(request, cancellationToken);

    await _sWLSignageRepository.UpdateAsync(sWLSignage.Mapping(request));

    return true;
  }

  private async Task<bool> CreateSWLSignageAsync(UpdateSWLCommand request, CancellationToken cancellationToken)
  {
    var result = await _sWLSignageRepository.AddAsync(request.ToEntity(), cancellationToken);

    return result != null;
  }
}
