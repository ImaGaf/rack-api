using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.End;

public class EndInspectionCommandHandler : ICommandHandler<EndInspectionCommand, Result>
{
  private readonly IInspectionService _inspectionService;
  public EndInspectionCommandHandler(IInspectionService inspectionService)
  {
    _inspectionService = inspectionService;
  }

  public async Task<Result> Handle(EndInspectionCommand request, CancellationToken cancellationToken)
  {
    var result = await _inspectionService.EndAsync(request, cancellationToken);
                  
    return result;
  }
}
