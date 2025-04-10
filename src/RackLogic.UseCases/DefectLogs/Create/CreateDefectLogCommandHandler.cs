using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Common.Extensions;
using RackLogic.UseCases.FloorPlanItems;
using RackLogic.UseCases.FloorPlans;

namespace RackLogic.UseCases.DefectLogs.Create;

public class CreateDefectLogCommandHandler(
  IRepositoryWithExpression<DefectLog> defectLogRepository,
  IFloorPlanItemService floorPlanItemService,
  IDefectLogHistoryService defectLogHistoryService,
  IFileService fileService)
  : ICommandHandler<CreateDefectLogCommand, Result<DefectLogDto>>
{
  public async Task<Result<DefectLogDto>> Handle(CreateDefectLogCommand request, CancellationToken cancellationToken)
  {
    var entity = request.ToEntity();

    entity.Id = Guid.NewGuid();

    entity.TicketId = GenerateTicketId();

    if (request.Files is not null && request.Files.Any())
    {
      var savedFiles = await fileService.SaveFilesAsync(
        entity.Id,
        request.Files,
        Path.Combine("wwwroot", "Warehouse/DefectLogs")
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

    await floorPlanItemService.UpdateInspectionDueDate(request.FloorPlanItemId);

    var historyDetails = DefectHistoryType.Created.GetDescription();
    await defectLogHistoryService.CreateAsync(entity, historyDetails, (int)DefectHistoryType.Created, cancellationToken);

    return Result.Success(entity.ToDto());
  }

  private string GenerateTicketId()
  {
    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 1_000_000_000;

    int random = new Random().Next(100, 999);

    return $"{timestamp:D9}{random:D3}";
  }
}
