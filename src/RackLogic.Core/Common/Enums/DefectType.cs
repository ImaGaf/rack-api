using System.ComponentModel.DataAnnotations;

namespace RackLogic.Core.Common.Enums;

public enum DefectType
{
  [Display(Name = "Bent / Buckled")]
  BentBuckled,
  Dent,
  Deflection,
  Misalignment,
  Unsecured,
  Missing,
  Corrosion,
  Other,
}
