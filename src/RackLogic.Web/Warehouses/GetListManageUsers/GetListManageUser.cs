using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RackLogic.UseCases.Warehouses.GetListManageUsers;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.Warehouses.GetListManageUsers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetListManageUser(IMediator _mediator) : Endpoint<GetManageUserRequest, ApiResponse<PageResponse<GetManageUserRespon>>>
{
  public override void Configure()
  {
    Get("/manager-user/list-manage-user/{WareHouseId}");
  }

  public override async Task HandleAsync(GetManageUserRequest req, CancellationToken ct)
  {
    var validator = new GetUserRequestValidator();
    var validationResult = validator.Validate(req);

    // Default value if PageNumber or PageSize is not passed
    int pageNumber = req.PageNumber ?? 1;
    int pageSize = req.PageSize ?? 10;

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new GetListManageUsersQuery(req.WareHouseId, pageNumber, pageSize), ct);
    if (result.IsNotFound())
    {
      throw new NotFoundException(result?.Errors?.FirstOrDefault());
    }
    if (result.IsSuccess)
    {
      // Mapper list of ManageUserDTOs to list of GetManageUserRespon
      var responseList = result.Value.Users
                    .Select(WareHouseMapper.ToGetManageUserRespon)
                    .ToList();

      Response = new ApiResponse<PageResponse<GetManageUserRespon>>(
                   new PageResponse<GetManageUserRespon>
                   {
                     PageNumber = pageNumber,
                     PageSize = pageSize,
                     TotalRecords = result.Value.TotalRecords,
                     Users = responseList
                   });

      return;
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
