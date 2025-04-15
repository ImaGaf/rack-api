using RackLogic.Core.Common.Enums;

namespace RackLogic.UseCases.DefectLogs.Count;

public class CountDefectLogDto
{
  public List<PriorityDto>? Priorities { get; set; }
  public List<StatusDto>? Statuses { get; set; }
}

public class PriorityDto
{
  public string? Type { get; set; }
  public int Count { get; set; }
}

public class StatusDto
{
  public string? Type { get; set; }
  public int Count { get; set; }
}
