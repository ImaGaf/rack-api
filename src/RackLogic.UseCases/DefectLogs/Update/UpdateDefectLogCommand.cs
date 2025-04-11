using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.DefectLogs.Update;

public record UpdateDefectLogCommand : ICommand<Result<DefectLogDto>>
{
  public required Guid Id { get; init; }
  public int? DefectStatus { get; init; }
  public string? Comment { get; init; }
}
