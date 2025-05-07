namespace RackLogic.UseCases.Inspections;

public interface IInspectionService
{
  Task<InspectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
