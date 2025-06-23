using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Constants;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections.ExportReport;

public class ExportReportCommandHandler(
  IInspectionService inspectionService,
  IUserWarehouseService userWarehouseService
) : ICommandHandler<ExportReportCommand, Result<byte[]>>
{
  public async Task<Result<byte[]>> Handle(ExportReportCommand command, CancellationToken cancellationToken)
  {
    if (userWarehouseService.HasPermission(PermissionConstant.Inspection.ViewReport, PermissionConstant.Inspection.ViewReport))
    {
      return Result.Forbidden(ErrorMessages.Inspection.ErrorMessageViewReport);
    }
    var pdfBytes = await inspectionService.ExportReportPdfAsync(command, cancellationToken);

    if (pdfBytes.Length == 0)
      return Result.Invalid(new ValidationError("Inspection not found"));

    return Result.Success(pdfBytes);
  }
}
