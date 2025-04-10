using RackLogic.Core.Common.Enums;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Core.Common.Entity;

public class DefectLogHistory : BaseEntityCustom
{
  public Guid DefectLogId { get; set; }
  public Guid UserId { get; set; }
  public string? Note { get; set; }
  public DefectHistoryType Type { get; set; }
  public virtual DefectLog? DefectLog { get; set; }
  public virtual ApplicationUser? User { get; set; }
}
