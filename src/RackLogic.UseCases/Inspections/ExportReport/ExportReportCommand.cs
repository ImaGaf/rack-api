using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.ExportReport;

public record ExportReportCommand : ICommand<Result<bool>>
{
  public required Guid InspectionId { get; set; }
}
