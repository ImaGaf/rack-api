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
public class GetListManageUser(IMediator _mediator) : Endpoint<GetManageUserRequest, PageResponse<GetManageUserRespon>>
{
  public override void Configure()
  {
    Get("/manager-user/listManageUser/{WarehouseId}");
  }

  public override async Task HandleAsync(GetManageUserRequest req, CancellationToken ct)
  {
    var validator = new GetUserRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }

    var result = await _mediator.Send(new GetListManageUsersQuery(req.WarehouseId, req.PageNumber, req.PageSize), ct);

    if (result.IsSuccess)
    {
      // Mapper list of ManageUserDTOs to list of GetManageUserRespon
      var responseList = result.Value.DataDTO
                    .Select(WareHouseMapper.ToGetManageUserRespon)
                    .ToList();

      Response = new PageResponse<GetManageUserRespon>
                   {
                     PageNumber = req.PageNumber,
                     PageSize = req.PageSize,
                     TotalRecords = result.Value.TotalRecords,
                     Data = responseList
                   };

      return;
    }
    await SendErrorsAsync(statusCode: 500, ct);
  }
}
