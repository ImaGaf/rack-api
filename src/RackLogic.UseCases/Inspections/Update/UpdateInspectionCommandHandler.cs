using Ardalis.Result;
using MediatR;

namespace RackLogic.UseCases.Inspections.Update;

public class UpdateInspectionCommandHandler : IRequestHandler<UpdateInspectionCommand, Result<InspectionDto>>
{
  private readonly IInspectionService _inspectionService;

  public UpdateInspectionCommandHandler(IInspectionService inspectionService)
  {
    _inspectionService = inspectionService;
  }

  async Task<Result<InspectionDto>> IRequestHandler<UpdateInspectionCommand, Result<InspectionDto>>.Handle(UpdateInspectionCommand request, CancellationToken cancellationToken)
  {
    var inspection = await _inspectionService.UpdateAsync(request, cancellationToken);

    if (inspection is null)
    {
      return Result<InspectionDto>.NotFound();
    }

    return Result<InspectionDto>.Success(inspection);
  }
}
