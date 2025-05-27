using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.ExportReport;

public record ExportReportCommand : ICommand<Result<byte[]>>
{
  public required Guid InspectionId { get; set; }
}
