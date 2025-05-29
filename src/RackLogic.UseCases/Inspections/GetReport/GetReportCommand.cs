using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.GetReport;

public record GetReportCommand : ICommand<Result<string>>
{
  public required Guid InspectionId { get; set; }
}
