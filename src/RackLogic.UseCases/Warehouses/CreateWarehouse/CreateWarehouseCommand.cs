using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.Warehouses.CreateWarehouse;
public record CreateWarehouseCommand (
  ClaimsPrincipal User,
  string CompanyName,
  string ABN,
  string SiteAddress,
  string ClientPhoneNumber,
  IFormFile FileImageLogo,
  string OwnerFirstName,
  string OwnerLastName,
  string OwnerPhoneNumber,
  string OwnerEmailAddress,
  bool IsNominatedInspector,
  string? InviteInspectorEmail)
  : ICommand<Result<bool>>;
