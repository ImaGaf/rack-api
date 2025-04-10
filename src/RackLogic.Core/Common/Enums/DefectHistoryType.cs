using System.ComponentModel.DataAnnotations;

namespace RackLogic.Core.Common.Enums;

public enum DefectHistoryType
{
  [Display(Description = "Defect recorded")]
  Created,
  Note,
  [Display(Description = "Updated to:")]
  StatusUpdate
}
