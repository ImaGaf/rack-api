using System.ComponentModel.DataAnnotations;
using Ardalis.SharedKernel;

namespace RackLogic.Core.Common.Entity;
public class BayConfiguration : EntityBase<Guid>, IAggregateRoot
{
  public Guid? ManufacturerId { get; set; }
  public OEMBrand? Manufacturer { get; set; }
  public Guid WarahouseId { get; set; }
  public Warehouse? Warehouses { get; set; }
  public DateTime CreatedDate { get; set; }
  public bool IsCustomBrand { get; set; }

  [Required, MaxLength(30)]
  public required string BayConfigId { get; set; }

  [Required]
  public required string RackingType { get; set; }
  public DateTime? InstallationDate { get; set; }
  public bool IsEstimatedInstallDate { get; set; }

  [Required]
  public required string ASComplianceVersion { get; set; }

  [Required, MaxLength(30)]
  public required string Supplier { get; set; }

  [Required, MaxLength(30)]
  public required string StructuralDesigner { get; set; }

  [Required, MaxLength(30)]
  public required string ProjectRef { get; set; }

  [Required, MaxLength(1)]
  public int ToleranceGrade { get; set; }

  [Required, MaxLength(5)]
  public int TotalBayLoad { get; set; }
  public string? Comment { get; set; }
  public bool IsEnableInterimInspection { get; set; }
  public string? InterimInterval { get; set; }
  public bool? HasSplice { get; set; }
  public int Level { get; set; }
  public int Pallets { get; set; }
  public bool HasProtector { get; set; }
  public string? SpliceKits { get; set; }
  public bool? IsCustomUpRight { get; set; }
  public bool? IsCustomBeam { get; set; }
  public bool? IsCustomBracingHorizontal { get; set; }
  public bool? IsCustomBracingDiagonal { get; set; }
  public bool? IsCustomRowSpacer { get; set; }
  public bool? IsCustomAccessories { get; set; }
  public int RowSpacerLevel { get; set; }
  public int BracingDiagonalLevel { get; set; }
  public int BracingLevel { get; set; }
  public int BeamLevel { get; set; }
  public List<Component> Components { get; set; } = new List<Component>();
  public List<BayConfigurationLevels> BayConfigurationLevels { get; set; } = new List<BayConfigurationLevels>();
}
