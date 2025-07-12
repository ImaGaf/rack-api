using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses.Specifications;

namespace RackLogic.UseCases.Warehouses.CreateWarehouse;

public class CreateWarehouseHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IFileService _fileService,
  IInvitationService _invitationService,
  UserManager<ApplicationUser> _userManager,
  RoleManager<IdentityRole<Guid>> _roleManager,
  IUser user
  )
  : ICommandHandler<CreateWarehouseCommand, Result<WarehouseDto>>
{
  public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
  {
    if (user.Id is null)
      return Result.Unauthorized();

    if (user.Role != RoleConstant.Premises && user.Role != RoleConstant.Inspector)
      return Result.Forbidden("You are not a premises or an inspector");

    if (user.Role == RoleConstant.Premises)
    {
      var hasWarehouse = await _userWarehouseRoleRepository
        .AnyAsync(new UserWarehouseRoleByUserSpec(user.Id.Value), cancellationToken);

      if (hasWarehouse)
        return Result.Error("You already have a warehouse.");
    }

    var creatorRoleId = await _roleManager.FindByNameAsync(user.Role);
    if (creatorRoleId == null) return Result.Error("Creator role is invalid");

    var warehouse = new Warehouse
    {
      Id = Guid.NewGuid(),
      CompanyName = request.CompanyName,
      OwnerId = user.Id.Value,
      ABN = request.ABN,
      Address = request.Address,
      AddressOptional = request.AddressOptional,
      City = request.City,
      PostCode = request.PostCode,
      State = request.State,
      IsGoogleApiAddress = request.IsGoogleApiAddress,
      ClientPhoneNumber = request.ClientPhoneNumber,
      OwnerFirstName = request.OwnerFirstName,
      OwnerLastName = request.OwnerLastName,
      OwnerPhoneNumber = request.OwnerPhoneNumber,
      OwnerEmailAddress = request.OwnerEmailAddress,
      UserWarehouseRoles = new List<UserWarehouseRole>(),
      FloorPlans = new List<FloorPlan>()
    };


    if (request.FileImageLogo != null && !await AddFileAsync(warehouse, request.FileImageLogo, cancellationToken))
    {
      return Result.Error("Failed to save logo file.");
    }

    await _warehouseRepository.AddToContextAsync(warehouse, cancellationToken: cancellationToken);

    // await _warehouseRepository.SaveEntityAsync(warehouse, cancellationToken);
    await _warehouseRepository.SaveContextAsync(cancellationToken);

    await _userWarehouseRoleRepository.AddAsync(new UserWarehouseRole
    {
      UserId = user.Id.Value,
      RoleId = creatorRoleId.Id,
      WarehouseId = warehouse.Id
    }, cancellationToken);

    if (request.IsNominatedInspector && !string.IsNullOrWhiteSpace(request.InviteInspectorEmail))
    {
      var inspector = await _userManager.FindByEmailAsync(request.InviteInspectorEmail);

      if (inspector != null && inspector.RegisteredRole != RoleConstant.Inspector)
        return Result.Invalid(new ValidationError("The invited user is not an Inspector."));

      await _invitationService.ProcessInvitationAsync(
        request.InviteInspectorEmail,
        warehouse.Id,
        RoleConstant.Inspector,
        cancellationToken
      );
    }

    return warehouse.ToDto();
  }

  private async Task<bool> AddFileAsync(Warehouse warehouse, IFormFile file, CancellationToken cancellationToken)
  {
    var savedFiles = await _fileService.SaveFilesAsync(warehouse.Id, null, [file]);
    var savedFile = savedFiles.FirstOrDefault();

    if (savedFile == null) return false;

    warehouse.FileImageUrl = savedFile.FilePath;
    warehouse.FileImageName = savedFile.FileName;

    return true;
  }
}
