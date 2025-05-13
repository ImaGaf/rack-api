using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.Inspections.SearchReport;

public class SearchInspectionReportQueryHandler(
  IInspectionService inspectionService,
  IUser user)
  : IQueryHandler<SearchInspectionReportQuery, Result<PagedDataDTO<InspectionReportDto>>>
{
  public async Task<Result<PagedDataDTO<InspectionReportDto>>> Handle(SearchInspectionReportQuery request, CancellationToken cancellationToken)
  {
    var userId = user.Id!.Value;
    var result = await inspectionService.SearchReportAsync(request, userId, cancellationToken);

    return Result.Success(result);
  }
}
