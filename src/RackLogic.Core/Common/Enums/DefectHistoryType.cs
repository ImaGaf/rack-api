using System.ComponentModel.DataAnnotations;

namespace RackLogic.Core.Common.Enums;

public enum DefectHistoryType
{
  [Display(Description = "Defect recorded")]
  Created,
  [Display(Description = "Defect recorded")]
  Note,
  [Display(Description = "Updated to:")]
  StatusUpdate,
  Completed
}
