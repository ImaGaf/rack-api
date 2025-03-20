using System.ComponentModel.DataAnnotations;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Enums;

namespace RackLogic.Core.Common.Entity;
public class DefaultSettings : EntityBase<Guid>, IAggregateRoot
{
  [Required]
  public Guid FloorPlanId { get; set; }
  public FloorPlan? FloorPlan { get; set; }
  [Required]
  public Guid BayConfigurationId { get; set; }
  public BayConfiguration? BayConfiguration { get; set; }
  [Required]
  public BayConfigurationCategory BayConfigCategory { get; set; }
}
