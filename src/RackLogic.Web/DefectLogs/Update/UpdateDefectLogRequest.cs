namespace RackLogic.Web.DefectLogs.Update;

public record UpdateDefectLogRequest
{
  public Guid? Id { get; init; }
  public int? DefectStatus { get; init; }
  public string? Comment { get; init; }
  public bool IsCompleted { get; init; }
}
