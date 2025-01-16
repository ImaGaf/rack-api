using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;

namespace RackLogic.UseCases.Warehouses.InviteUser;
public class InviteUserHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IRepositoryWithExpression<Invitation> _invitationsRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  RoleManager<IdentityRole<Guid>> _roleManager,
  UserManager<ApplicationUser> _userManager,
  IConfiguration _configuration,
  IEmailSender _emailSender) 
  : ICommandHandler<InviteUserCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(InviteUserCommand request, CancellationToken cancellationToken)
  {
    if (!request.User.Identity?.IsAuthenticated ?? false) return Result.Unauthorized();

    var inviteIdString = request.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    var inviteId = Guid.Parse(inviteIdString);

    if (!await CheckPermissionUserRequest(inviteId, request.WarehouseId)) return Result.Forbidden();

    var role = await _roleManager.FindByNameAsync(request.Role);
    if (role is null) return Result.Invalid(new ValidationError("Role invalid."));

    // Check warehouse in database
    var result = await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId);
    if (result is null) return Result.Invalid(new ValidationError("Warehouse not found."));

    var user = await _userManager.FindByEmailAsync(request.Email);
    var userName = request.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    var baseUrl = _configuration.GetSection("Dashboard:InviteUser").Value;
    var logo = $"{baseUrl}{_configuration.GetSection("Dashboard:LogoMarket").Value}";

    if (user is null)
    {
      var invitation = await _invitationsRepository.AddAsync(
        new Invitation() 
        {
          Role = request.Role,
          Email = request.Email,
          InviteById = inviteId,
          WarehouseId = request.WarehouseId,
        }, cancellationToken);

      var linkUrl = $"{baseUrl}email={invitation.Email},inviterole={request.Role}";
      var content = BuildEmailInviteContent(linkUrl, logo, userName, result.CompanyName, role?.Name);
      return await _emailSender.SendAsync(request.Email, EmailConstants.InviteSubject, content);
    }

    if (request.Role == RoleConstant.Inspector && user.RegisteredRole != RoleConstant.Inspector)
      return Result.Invalid(new ValidationError("The user is not an inspector."));

    if (request.Role == RoleConstant.Repairer && user.RegisteredRole != RoleConstant.Repairer)
      return Result.Invalid(new ValidationError("The user is not an repairer."));

    await _userWarehouseRoleRepository.AddAsync(
     new UserWarehouseRole()
     {
       UserId = user.Id,
       WarehouseId = request.WarehouseId,
       RoleId = role.Id,
     }, cancellationToken);

    var contentEmail = BuildEmailInviteExistedUserContent(logo, userName, result.CompanyName, role?.Name);
    return await _emailSender.SendAsync(request.Email, EmailConstants.InviteExistedUserSubject, contentEmail);
  }

  private string BuildEmailInviteContent( string link, string logo, string inviteName, string locationName, string? roleName)
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

  private async Task<bool> CheckPermissionUserRequest(Guid userId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises && role.Name != RoleConstant.ClientManager) return false;

    return true;
  }
}
