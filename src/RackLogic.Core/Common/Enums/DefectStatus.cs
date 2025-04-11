using System.ComponentModel.DataAnnotations;

namespace RackLogic.Core.Common.Enums;

public enum DefectStatus
{
  [Display(Description = "Not Started")]
  NotStarted,

  [Display(Description = "In Progress")]
  InProgress,

  [Display(Description = "Awaiting Component")]
  AwaitingComponent,

  [Display(Description = "Repair Scheduled")]
  RepairScheduled,

  [Display(Description = "Final")]
  Final
}
