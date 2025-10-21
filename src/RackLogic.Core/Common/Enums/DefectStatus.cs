using System.ComponentModel.DataAnnotations;

namespace RackLogic.Core.Common.Enums;

public enum DefectStatus
{
  [Display(Description = "Action Required")]
  ActionRequired = 0,

  [Display(Description = "In Progress")]
  InProgress = 1,

  [Display(Description = "No Action Required")]
  RemediateRepair = 2,

  [Display(Description = "Awaiting Component")]
  AwaitingComponent = 3,

  [Display(Description = "Repair Scheduled")]
  RepairScheduled = 4,

  [Display(Description = "Ready For Inspection")]
  ReadyForInspection = 5,
}
