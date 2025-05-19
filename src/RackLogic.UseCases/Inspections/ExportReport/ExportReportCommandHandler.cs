using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.ExportReport;

public class ExportReportCommandHandler(
  IInspectionService inspectionService
) : ICommandHandler<ExportReportCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(ExportReportCommand command, CancellationToken cancellationToken)
  {
    var pdfBytes = await inspectionService.ExportReportPdfAsync(command, cancellationToken);

    if (pdfBytes.Length == 0)
      return Result.Invalid(new ValidationError("Inspection not found"));

    var filePath = Path.Combine("wwwroot", "exports", $"InspectionReport_{command.InspectionId}.pdf");
    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    await File.WriteAllBytesAsync(filePath, pdfBytes, cancellationToken);

    return Result.Success(true);
  }
}
