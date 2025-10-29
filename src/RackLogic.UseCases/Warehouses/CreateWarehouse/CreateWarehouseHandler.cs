using System.Threading;
using Ardalis.GuardClauses;
using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
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
    Guard.Against.Null(user.Id);
    if (user.Role == RoleConstant.Premises
      && await ExistsWarehouse(user.Id.Value, cancellationToken))
    {
      return Result.Error("You already have a warehouse.");
    }

    // Invite Premises from Inspector Account
    if (IsInvite(request.OwnerEmailAddress))
    {
      var dbUser = await _userManager.FindByEmailAsync(request.OwnerEmailAddress ?? string.Empty);
      if (dbUser != null
        && dbUser.RegisteredRole == RoleConstant.Premises
        && await ExistsWarehouse(dbUser.Id, cancellationToken))
      {
        return Result.Invalid(new ValidationError("The invited user is not an Premises."));
      }
    }

    var creatorRoleId = await _roleManager.FindByNameAsync(user.Role ?? string.Empty);

    ///
    /// warning: should generate UUID v7 using Guid.CreateVersion7() which was introduced in .NET 9
    /// 
    var warehouseId = Guid.NewGuid();
    var warehouse = new Warehouse
    {
      Id = warehouseId,
      CompanyName = request.CompanyName,
      OwnerId = user.Role == RoleConstant.Inspector
        && IsInvite(request.OwnerEmailAddress)
        ? null
        : user.Id,
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
      UserWarehouseRoles = new List<UserWarehouseRole>()
      {
        new UserWarehouseRole
        {
          UserId = user.Id.Value,
          RoleId = creatorRoleId!.Id,
          WarehouseId = warehouseId,
        },
      },
      FloorPlans = new List<FloorPlan>()
    };


    if (request.FileImageLogo != null)
    {
      var savedFiles = await _fileService.SaveFilesAsync(warehouse.Id, null, [request.FileImageLogo]);
      warehouse.FileImageUrl = savedFiles.FirstOrDefault()?.FilePath;
      warehouse.FileImageName = savedFiles.FirstOrDefault()?.FileName;
    }

    await _warehouseRepository.AddToContextAsync(
      warehouse,
      autoSave: true,
      cancellationToken);

    if (IsInvite(request.OwnerEmailAddress))
    {
      await _invitationService.ProcessInvitationAsync(
        request.OwnerEmailAddress ?? string.Empty,
        warehouseId,
        RoleConstant.Premises,
        cancellationToken
      );
    }

    return warehouse.ToDto();
  }

  private async Task<bool> ExistsWarehouse(Guid userId, CancellationToken cancellationToken)
    => await _userWarehouseRoleRepository
    .AnyAsync(new UserWarehouseRoleByUserSpec(userId), cancellationToken);

  private bool IsInvite(string? ownerEmailAddress)
    => user.Role == RoleConstant.Inspector
      && !string.IsNullOrWhiteSpace(ownerEmailAddress);
}
