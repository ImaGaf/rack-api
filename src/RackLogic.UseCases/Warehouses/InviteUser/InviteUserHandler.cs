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
  IConfiguration _configuration) 
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
      var baseUrl = _configuration.GetSection("Dashboard:InviteUser").Value;
      var logo = $"{baseUrl}{_configuration.GetSection("Dashboard:LogoMarket").Value}";
      var linkUrl = $"{baseUrl}email={invitation.Email},inviterole={request.Role}";
      var content = BuildEmailInviteContent(linkUrl, logo);
      //TODO: Send Email 
      //var result = await _emailSender.SendAsync(request.Email, EmailConstants.OTPSubject, content);
      return true;
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
    // TODO: Send Email notification.
    return true;
  }

  private string BuildEmailInviteContent( string link, string logo)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "InviteUser.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);

    //emailContent = emailContent.Replace("{platformName}", platformName);
    emailContent = emailContent.Replace("{link}", link);
    emailContent = emailContent.Replace("{logo}", logo);
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
