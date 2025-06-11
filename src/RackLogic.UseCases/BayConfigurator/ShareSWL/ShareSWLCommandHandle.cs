using Ardalis.Result;
using MediatR;

namespace RackLogic.UseCases.BayConfigurator.ShareSWL;

public class ShareSWLCommandHandle : IRequestHandler<ShareSWLCommand, Result>
{
  private readonly IBayConfigurationService _bayConfigurationService;

  public ShareSWLCommandHandle(IBayConfigurationService bayConfigurationService)
  {
    _bayConfigurationService = bayConfigurationService;
  }

  public async Task<Result> Handle(ShareSWLCommand request, CancellationToken cancellationToken)
  {
    await _bayConfigurationService.ExportSWLAsync(request, cancellationToken);

    return Result.Success();
  }
}
