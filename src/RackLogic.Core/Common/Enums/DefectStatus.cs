using System.ComponentModel.DataAnnotations;

namespace RackLogic.Core.Common.Enums;

public enum DefectStatus
{
  ActionRequired = 0,
  InProgress = 1,
  RemediateRepair = 2,
  AwaitingComponent = 3,
  RepairScheduled = 4,
  ReadyForInspection = 5,
}
