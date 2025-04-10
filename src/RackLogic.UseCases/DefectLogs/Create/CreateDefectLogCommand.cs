using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.DefectLogs.Create;

public record CreateDefectLogCommand : ICommand<Result<DefectLogDto>>
{
  public required Guid ComponentId { get; init; }
  public required Guid FloorPlanItemId { get; init; }
  public Guid? RelatedComponentId { get; init; }
  public required int DefectType { get; init; }
  public required int DefectGrade { get; init; }
  public required int CorrectiveAction { get; init; }
  public required int DefectPriority { get; init; }
  public required int DefectStatus { get; init; }
  public required int ComponentIndex { get; init; }
  public string? DrawPosition { get; init; }
  public bool? IsRear { get; init; }
  public string? Comment { get; init; }
  public List<IFormFile>? Files { get; init; }
}
