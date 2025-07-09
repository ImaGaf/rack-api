using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.Warehouses.CreateWarehouse;
public record CreateWarehouseCommand (
  string CompanyName,
  string ABN,
  string Address,
  string? AddressOptional,
  string? City,
  string? State,
  string? PostCode,
  bool? IsGoogleApiAddress,
  string ClientPhoneNumber,
  IFormFile? FileImageLogo,
  string OwnerFirstName,
  string OwnerLastName,
  string OwnerPhoneNumber,
  string OwnerEmailAddress,
  bool IsNominatedInspector,
  string? InviteInspectorEmail)
  : ICommand<Result<WarehouseDto>>;
