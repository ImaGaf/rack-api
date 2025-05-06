using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.Get;

public record GetInspectionByIdQuery(Guid InspectionId) : IQuery<Result<InspectionDto>>;
