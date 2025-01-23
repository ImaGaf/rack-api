using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.FloorPlans;

namespace RackLogic.UseCases.Warehouses.CreateWarehouse;
public class CreateWarehouseHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IRepositoryWithExpression<Invitation> _invitationsRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  RoleManager<IdentityRole<Guid>> _roleManager,
  IFileService _fileService,
  UserManager<ApplicationUser> _userManager,
  IConfiguration _configuration,
  IEmailSender _emailSender)
  : ICommandHandler<CreateWarehouseCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
  {
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();
    var createIdString = request.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var creatorId = Guid.Parse(createIdString);
    var creatorRole = request.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
    var creatorRoleId = await _roleManager.FindByNameAsync(creatorRole);
    var inspectorRole = await _roleManager.FindByNameAsync(RoleConstant.Inspector);

    if (!await CheckPermissionUserRequest(creatorId)) return Result.Forbidden();

    //If user switch on button and user not a inspector
    if (request.IsNominatedInspector && creatorRole != RoleConstant.Inspector) return Result.Invalid(new ValidationError("Only Inspectors can be the nominated inspector."));

    if (inspectorRole is null || creatorRoleId is null) return Result.Invalid(new ValidationError("Inspector role or Premises role not found."));

    //Create Warehouse
    var warehouse = new Warehouse
    {
      Id = Guid.NewGuid(),
      CompanyName = request.CompanyName,
      OwnerId = creatorId,
      ABN = request.ABN,
      SiteAddress = request.SiteAddress,
      ClientPhoneNumber = request.ClientPhoneNumber,
      OwnerFirstName = request.OwnerFirstName,
      OwnerLastName = request.OwnerLastName,
      OwnerPhoneNumber = request.OwnerPhoneNumber,
      OwnerEmailAddress = request.OwnerEmailAddress,
      UserWarehouseRoles = new List<UserWarehouseRole>(),
      FloorPlans = new List<FloorPlan>()
    };

    await _warehouseRepository.AddToContextAsync(warehouse);

    // Call func Savefile
    if (!await AddFileAsync(warehouse, request.FileImageLogo, cancellationToken)) return Result.Error("Failed to save logo file.");

    //Create UserWarehouseRole
    if (!await AddUserWarehouseRoleAsync(creatorId, warehouse.Id, creatorRoleId.Id, cancellationToken)) return Result.Error("Failed to assign role in warehouse.");
    
    //If user switch off button
    if (!request.IsNominatedInspector && !await InviteUserAsync(request, warehouse, inspectorRole.Id, creatorId, cancellationToken)) return Result.Invalid(new ValidationError("The invited user is not an Inspector."));

    await _warehouseRepository.SaveEntityAsync(warehouse, cancellationToken);
    await _warehouseRepository.SaveContextAsync(cancellationToken);
    return Result.Success(true);
  }

  //Funcition
  //Invite user to warehouse
  private async Task<bool> InviteUserAsync(CreateWarehouseCommand request, Warehouse warehouse, Guid inspectorRoleId, Guid creatorId, CancellationToken cancellationToken)
  {
    // If the invitee's email is not filled in
    if (string.IsNullOrEmpty(request.InviteInspectorEmail)) return true;

    var invitedUser = await _userManager.FindByEmailAsync(request.InviteInspectorEmail!);
    var userName = request.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    var baseUrl = _configuration.GetSection("Dashboard:InviteUser").Value;
    var logo = $"{baseUrl}{_configuration.GetSection("Dashboard:LogoMarket").Value}";

    // If the invited user does not exist
    if (invitedUser is null)
    {
      var invitation = new Invitation()
      {
        Role = RoleConstant.Inspector,
        Email = request.InviteInspectorEmail!,
        InviteById = creatorId,
        WarehouseId = warehouse.Id,
      };

      await _invitationsRepository.AddToContextAsync(invitation);

      var linkUrl = $"{baseUrl}email={invitation.Email},inviterole={RoleConstant.Inspector}";
      var content = BuildEmailInviteContent(linkUrl, logo, userName, warehouse.CompanyName, RoleConstant.Inspector);

      // Send user invitation email
      return await _emailSender.SendAsync(request.InviteInspectorEmail!, EmailConstants.InviteSubject, content);
    }

    // Check the role of the invitee
    if (invitedUser.RegisteredRole != RoleConstant.Inspector) return false;

    // Create UserWarehouseRole
    if (!await AddUserWarehouseRoleAsync(invitedUser.Id, warehouse.Id, inspectorRoleId, cancellationToken)) return false;

    var contentEmail = BuildEmailInviteExistedUserContent(logo, userName, warehouse.CompanyName, RoleConstant.Inspector);
    return await _emailSender.SendAsync(request.InviteInspectorEmail!, EmailConstants.InviteExistedUserSubject, contentEmail);
  }

  //Save file folder and add file path in db
  private async Task<bool> AddFileAsync(Warehouse warehouse, IFormFile file, CancellationToken cancellationToken)
  {
    var savedFiles = await _fileService.SaveFilesAsync(warehouse.Id, null, new List<IFormFile> { file });
    var savedFile = savedFiles.FirstOrDefault();

    if (savedFile == null) return false;

    warehouse.FileImageUrl = savedFile.FilePath;
    warehouse.FileImageName = savedFile.FileName;
    await _warehouseRepository.UpdateToContextAsync(warehouse);

    return true;
  }

  //Add data to UserWarehouseRole table
  private async Task<bool> AddUserWarehouseRoleAsync(Guid userId, Guid warehouseId, Guid roleId, CancellationToken cancellationToken)
  {
    var userWarehouseRole = new UserWarehouseRole
    {
      UserId = userId,
      WarehouseId = warehouseId,
      RoleId = roleId,
    };

    await _userWarehouseRoleRepository.AddToContextAsync(userWarehouseRole);
    return true;
  }

  private string BuildEmailInviteContent(string link, string logo, string inviteName, string locationName, string? roleName)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "InviteUser.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);
    emailContent = emailContent.Replace("{link}", link);
    emailContent = emailContent.Replace("{logo}", logo);
    emailContent = emailContent.Replace("{invited_name}", inviteName);
    emailContent = emailContent.Replace("{location_name}", locationName);
    emailContent = emailContent.Replace("{role_name}", roleName);
    emailContent = emailContent.Replace("{currentYear}", DateTime.Now.Year.ToString());

    return emailContent;
  }

  private string BuildEmailInviteExistedUserContent(string logo, string inviteName, string locationName, string? roleName)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "InviteUserHasExisted.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);

    emailContent = emailContent.Replace("{logo}", logo);
    emailContent = emailContent.Replace("{invited_name}", inviteName);
    emailContent = emailContent.Replace("{location_name}", locationName);
    emailContent = emailContent.Replace("{role_name}", roleName);
    emailContent = emailContent.Replace("{currentYear}", DateTime.Now.Year.ToString());

    return emailContent;
  }

  private async Task<bool> CheckPermissionUserRequest(Guid userId)
  {
    var user = await _userManager.FindByIdAsync(userId.ToString());

    if (user == null) return false;

    if (user.RegisteredRole != RoleConstant.Premises && user.RegisteredRole != RoleConstant.Inspector) return false;

    return true;
  }
}
