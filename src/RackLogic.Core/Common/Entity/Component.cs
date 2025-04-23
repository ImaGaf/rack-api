using System.ComponentModel.DataAnnotations;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Enums;

namespace RackLogic.Core.Common.Entity;
public class Component : EntityBase<Guid>, IAggregateRoot
{
  public Guid BayId { get; set; }
  [Required]
  public required ComponentType ComponentType { get; set; }
  public int? Height { get; set; }
  public int? Width { get; set; }
  public int? Length { get; set; }
  public string? Code { get; set; }
  public string? Description { get; set; }
  public string? Orientation { get; set; }
  public double? DistanceCenterToCenter { get; set; }
  public ComponentCategory? Category { get; set; }
  public int? Quantity { get; set; }
  public string? Notes { get; set; }
  public int? LowerHeight { get; set; }
  public int? UpperHeight { get; set; }
  public bool? IsXBrace { get; set; }
  public int? Kit { get; set; }
  public string? ComponentKit { get; set; }
  public string? FinishType { get; set; }
  public string? BeamStyle { get; set; }
  public int? MaterialThickness { get; set; }
  public string? ClearEntryWidth { get; set; }
  public string? ConnectorType { get; set; }
  public virtual BayConfiguration? BayConfiguration { get; set; }
}
