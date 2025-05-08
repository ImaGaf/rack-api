using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Inspections.Get;

public class GetInspectionByIdQueryHandler(
  IInspectionService inspectionService)
  : IQueryHandler<GetInspectionByIdQuery, Result<InspectionDto>>
{
  public async Task<Result<InspectionDto>> Handle(GetInspectionByIdQuery request, CancellationToken cancellationToken)
  {
    var result = await inspectionService.GetByIdAsync(request.InspectionId, cancellationToken);

    if (result == null)
    {
      return Result.NotFound("Inspection not found");
    }

    return Result.Success(result);
  }
}
