using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.GetReport;

public class GetReportCommandHandler(
  IInspectionService inspectionService
) : ICommandHandler<GetReportCommand, Result<string>>
{
  public async Task<Result<string>> Handle(GetReportCommand command, CancellationToken cancellationToken)
  {
    var html = await inspectionService.GetReportHtmlAsync(command.InspectionId, cancellationToken);

    return Result.Success(html);
  }
}
