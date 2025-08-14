using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.Common.Extensions;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.DefectLogs.Update;

public class UpdateDefectLogCommandHandler(
  IRepositoryWithExpression<DefectLog> defectLogRepository,
  IDefectLogService defectLogService,
  IDefectLogHistoryService defectLogHistoryService,
  IUserWarehouseService userWarehouseService)
  : ICommandHandler<UpdateDefectLogCommand, Result<DefectLogDto>>
{
  public async Task<Result<DefectLogDto>> Handle(UpdateDefectLogCommand request, CancellationToken cancellationToken)
  {
    if (!userWarehouseService.HasPermission(PermissionConstant.DefectLog.Code, PermissionConstant.DefectLog.UpdateStatus))
    {
      return Result.Forbidden(ErrorMessages.DefectLog.ErrorMessageUpdate);
    }

    if (!userWarehouseService.HasPermission(PermissionConstant.DefectLog.Code, PermissionConstant.DefectLog.Resolve))
    {
      return Result.Forbidden(ErrorMessages.DefectLog.ErrorMessageResolve);
    }

    var entity = await defectLogService.GetByIdAsync(request.Id, cancellationToken);

    if (entity == null)
      return Result.NotFound("Defect Log not found.");

    // Update Status
    if (request.DefectStatus != null && entity.DefectStatus != (DefectStatus)request.DefectStatus && !entity.IsCompleted)
    {
      entity.DefectStatus = (DefectStatus)request.DefectStatus;

      var newStatusDescription = ((DefectStatus)request.DefectStatus!).GetDescription();
      var historyDetails = $"{DefectHistoryType.StatusUpdate.GetDescription()} {newStatusDescription}";

      await defectLogHistoryService
        .CreateAsync(
          entity,
          historyDetails,
          (int)DefectHistoryType.StatusUpdate,
          cancellationToken);
    }

    // Update Note
    if (!string.IsNullOrEmpty(request.Comment) && request.Comment != entity.Comment)
    {
      await defectLogHistoryService
        .CreateAsync(
          entity,
          request.Comment,
          (int)DefectHistoryType.Note,
          cancellationToken);
    }

    // Return if the defect log is already completed
    if (!request.IsCompleted ?? false)
      return Result.Error("This defect have already completed.");

    // Resolve Defect Log
    if (request.IsCompleted != null && request.IsCompleted.Value)
    {
      entity.IsCompleted = request.IsCompleted.Value;

      await defectLogHistoryService
        .CreateAsync(
          entity,
          "Resolved",
          (int)DefectHistoryType.Completed,
          cancellationToken);
    }

    await defectLogRepository.UpdateToContextAsync(entity, true, cancellationToken);

    var brand = await userWarehouseService.GetBrandByComponentAsync(entity.ComponentId);

    var result = entity.ToDtoWithHistory();
    result.OEMBrand = brand;

    return Result.Success(result);
  }
}
