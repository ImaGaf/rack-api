using System.ComponentModel.DataAnnotations;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Enums;

namespace RackLogic.Core.Common.Entity;
public class DefaultSettings : EntityBase<Guid>, IAggregateRoot
{
  [Required]
  public Guid FloorPlanId { get; set; }
  public Guid? BayConfigurationId { get; set; }
  [Required]
  public BayConfigurationCategory BayConfigCategory { get; set; }
  public virtual FloorPlan? FloorPlan { get; set; }
  public virtual BayConfiguration? BayConfiguration { get; set; }
}
