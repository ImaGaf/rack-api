namespace RackLogic.Core.Common.Entity;

public class DefectLogFile : BaseEntityCustom
{
  public Guid DefectLogId { get; set; }
  public virtual DefectLog? DefectLog { get; set; }
  public string? FileName { get; set; }
  public string? FileUrl { get; set; }
  public string? ContentType { get; set; }
}
