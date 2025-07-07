using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.Common.Extensions;
using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.DefectLogs.Create;

public class CreateDefectLogCommandHandler(
  IRepositoryWithExpression<DefectLog> defectLogRepository,
  IRepositoryWithExpression<FloorPlanItem> floorPlanItemRepository,
  IUserWarehouseService userWarehouseService,
  IDefectLogHistoryService defectLogHistoryService,
  IFileService fileService)
  : ICommandHandler<CreateDefectLogCommand, Result<DefectLogDto>>
{
  public async Task<Result<DefectLogDto>> Handle(CreateDefectLogCommand request, CancellationToken cancellationToken)
  {
    if (!userWarehouseService.HasPermission(PermissionConstant.DefectLog.Code, PermissionConstant.DefectLog.Create))
    {
      return Result.Forbidden(ErrorMessages.DefectLog.ErrorMessageCreate);
    }

    var entity = request.ToEntity();

    entity.Id = Guid.NewGuid();

    entity.TicketId = GenerateTicketId();

    if (request.Files is not null && request.Files.Any())
    {
      var savedFiles = await fileService.SaveFilesAsync(
        entity.Id,
        request.Files,
        Path.Combine("wwwroot", "DefectLogs")
      );

      var fileEntities = savedFiles
          .Select(f => new DefectLogFile
          {
            Id = Guid.NewGuid(),
            DefectLogId = entity.Id,
            FileName = f.FileName,
            FileUrl = f.FilePath,
            ContentType = f.ContentType
          }).ToList();

      entity.DefectLogFiles = fileEntities;
    }

    await defectLogRepository.AddAsync(entity, cancellationToken: cancellationToken);

    var historyDetails = DefectHistoryType.Created.GetDescription();
    await defectLogHistoryService.CreateAsync(entity, historyDetails, (int)DefectHistoryType.Created, cancellationToken);

    var brand = await userWarehouseService.GetBrandByComponentAsync(request.ComponentId);

    var floorPlanItem = await floorPlanItemRepository.FirstOrDefaultAsync(x => x.Id == request.FloorPlanItemId, cancellationToken, x => x.BayConfiguration);

    var result = entity.ToDto();

    result.BayConfigId = floorPlanItem?.BayConfiguration?.BayConfigId;

    result.OEMBrand = brand;

    return Result.Success(result);
  }

  private string GenerateTicketId()
  {
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 1_000_000_000;

    int random = new Random().Next(100, 999);

    return $"{timestamp:D9}{random:D3}";
  }
}
