using Ardalis.SharedKernel;
using RackLogic.Core.Common.Enums;

namespace RackLogic.Core.Common.Entity;
public class BayConfigurationLevels : EntityBase<Guid>, IAggregateRoot
{
  public Guid BayId { get; set; }
  public BayConfiguration? BayConfiguration { get; set; }
  public required ComponentType LevelType { get; set; }
  public int? LevelValue { get; set; }
  public string? Orientation { get; set; }
  public int? LowerHeight { get; set; }
  public int? UpperHeight { get; set; }
  public bool? IsXBrace { get; set; }
}
