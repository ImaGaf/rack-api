using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Inspections.Delete;

public class DeleteInspectionCommandHandler(
  IRepositoryWithExpression<Inspection> _inspectionRepository,
  IUser _user)
  : ICommandHandler<DeleteInspectionCommand, Result>
{
  public async Task<Result> Handle(DeleteInspectionCommand request, CancellationToken cancellationToken)
  {
    var inspection = await _inspectionRepository.GetByIdAsync(request.id, cancellationToken);

    if (inspection is null)
      return Result.NotFound();

    inspection.IsDeleted = true;
    inspection.DeleterId = _user.Id;

    await _inspectionRepository.UpdateAsync(inspection, cancellationToken);

    return Result.Success();
  }
}
