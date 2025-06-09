using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;

namespace RackLogic.UseCases.Inspections.GetAll;

public class GetAllInspectionQueryHandler : IQueryHandler<GetAllInspectionQuery, Result<IEnumerable<InspectionDto>>>
{
  private readonly IInspectionService _inspectionService;

  public GetAllInspectionQueryHandler(IInspectionService inspectionService)
  {
    _inspectionService = inspectionService;
  }

  async Task<Result<IEnumerable<InspectionDto>>> IRequestHandler<GetAllInspectionQuery, Result<IEnumerable<InspectionDto>>>.Handle(GetAllInspectionQuery request, CancellationToken cancellationToken)
  {
    var inspection = await _inspectionService.GetAllAsync(request.WarehouseId, cancellationToken);

    return Result.Success(inspection);
  }
}

