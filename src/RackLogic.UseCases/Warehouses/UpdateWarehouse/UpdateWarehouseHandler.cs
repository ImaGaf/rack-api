using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.FloorPlans;

namespace RackLogic.UseCases.Warehouses.UpdateWarehouse;
public class UpdateWarehouseHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IFileService _fileService) : ICommandHandler<UpdateWarehouseCommand, Result<WarehouseDto>>
{
  public async Task<Result<WarehouseDto>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
  {
    var warehouse = await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId);

    if (warehouse is null) return Result.Invalid(new ValidationError("Warehouse not found."));

    await HandleFileAsync(request, warehouse);
    await UpdateWarehouseAsync(request, warehouse);

    return warehouse.ToDto();
  }

  private async Task HandleFileAsync(UpdateWarehouseCommand request, Warehouse warehouse)
  {
    if (request.FileImageLogo is null) return;

    await _fileService.DeleteFilesAsync(new List<string>() { $"{warehouse.FileImageUrl}"});
    var saveFilesResult = await _fileService.SaveFilesAsync(request.WarehouseId, null, new List<Microsoft.AspNetCore.Http.IFormFile>() { request.FileImageLogo });
    
    if (saveFilesResult is null) return;

    var result = saveFilesResult.FirstOrDefault();
    warehouse.FileImageUrl = result?.FilePath;
    warehouse.FileImageName = result?.FileName;
  }

  private async Task UpdateWarehouseAsync(UpdateWarehouseCommand request, Warehouse warehouse)
  {
    warehouse.CompanyName = request.CompanyName;
    warehouse.ABN = request.ABN;
    warehouse.Address = request.Address;
    warehouse.AddressOptional = request.AddressOptional;
    warehouse.City = request.City;
    warehouse.State = request.State;
    warehouse.PostCode = request.PostCode;
    warehouse.IsGoogleApiAddress = request.IsGoogleApiAddress;
    warehouse.ClientPhoneNumber = request.ClientPhoneNumber;
    warehouse.OwnerFirstName = request.OwnerFirstName;
    warehouse.OwnerLastName = request.OwnerLastName;
    warehouse.OwnerEmailAddress = request.OwnerEmailAddress;
    warehouse.OwnerPhoneNumber = request.OwnerPhoneNumber;
    await _warehouseRepository.UpdateAsync(warehouse);
  }
}
