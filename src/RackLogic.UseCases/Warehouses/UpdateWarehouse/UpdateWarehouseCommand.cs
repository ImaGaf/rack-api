using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.Warehouses.UpdateWarehouse;
public record UpdateWarehouseCommand(
  Guid WarehouseId,
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
  string OwnerEmailAddress)
  : ICommand<Result<WarehouseDto>>;
