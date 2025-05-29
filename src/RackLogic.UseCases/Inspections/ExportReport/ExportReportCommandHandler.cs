using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.ExportReport;

public class ExportReportCommandHandler(
  IInspectionService inspectionService
) : ICommandHandler<ExportReportCommand, Result<byte[]>>
{
  public async Task<Result<byte[]>> Handle(ExportReportCommand command, CancellationToken cancellationToken)
  {
    var pdfBytes = await inspectionService.ExportReportPdfAsync(command, cancellationToken);

    if (pdfBytes.Length == 0)
      return Result.Invalid(new ValidationError("Inspection not found"));

    return Result.Success(pdfBytes);
  }
}
