using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.Infrastructure.Data.Queries;

public class InvitationService(
  AppDbContext _context,
  RoleManager<IdentityRole<Guid>> _roleManager,
  IRepositoryWithExpression<Invitation> _invitationsRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<WarehouseFinalUses> _warehouseFinalUseRepository,
  UserManager<ApplicationUser> _userManager,
  IConfiguration _configuration,
  IEmailSender _emailSender,
  IUser _user)
  : IInvitationService
{
  public async Task<Result<bool>> ProcessInvitationAsync(
    string email,
    Guid warehouseId,
    string inviteRole,
    CancellationToken ct)
  {
    var role = await _roleManager.FindByNameAsync(inviteRole);
    if (role is null)
      return Result.Invalid(new ValidationError("Role invalid."));

    var warehouse = await _context.Warehouses.FindAsync(warehouseId);
    if (warehouse is null)
      return Result.Invalid(new ValidationError("Warehouse not found."));

    var invitedUser = await _userManager.FindByEmailAsync(email);

    var inviterId = _user.Id!.Value;
    var inviterName = _user.UserName ?? "Someone";

    var deepLink = _configuration["Dashboard:InviteUser"];
    var baseUrl = _configuration["Client:BaseUrl"];
    var logo = $"{baseUrl}{_configuration["Client:LogoMarket"]}";

    if (invitedUser is null)
    {
      var result = await HandlerUserNotExistsAsync(
        email, warehouseId, inviteRole, _user.Id!.Value, deepLink, logo,
        inviterName, warehouse.CompanyName, role.Name, baseUrl, ct);

      return result
        ? Result.Success(true)
        : Result.Error("Failed to send invitation.");
    }

    return await HandlerUserExistsAsync(
      email, warehouseId, inviteRole, invitedUser, role, logo,
      inviterName, warehouse.CompanyName, ct);
  }

  public async Task AcceptInvitationAsync(string emailInvited, Guid warehouseId, ApplicationUser user, CancellationToken cancellationToken)
  {
    var isRepairerOrInspector = user.RegisteredRole == RoleConstant.Repairer
      || user.RegisteredRole == RoleConstant.Inspector;

      var invitations = isRepairerOrInspector
      ? await _invitationsRepository
        .FindAsync(x => x.Email == emailInvited, cancellationToken)
      : await _invitationsRepository
        .FindAsync(x => x.Email == emailInvited && x.WarehouseId == warehouseId);

    foreach (var item in invitations)
    {
      var role = await _roleManager.FindByNameAsync(item.Role);
      if (role == null) continue;

      await _userWarehouseRoleRepository.AddAsync(new UserWarehouseRole
      {
        UserId = user.Id,
        RoleId = role.Id,
        WarehouseId = item.WarehouseId
      }, cancellationToken);

      if (!isRepairerOrInspector)
      {
        user.ParentId = item.InviteById;
        await _userManager.UpdateAsync(user);
      }

      await _invitationsRepository.DeleteAsync(item, cancellationToken);
      await _warehouseFinalUseRepository.AddAsync(
        new WarehouseFinalUses()
        {
          UserId = user.Id,
          WarehouseId = item.WarehouseId,
        },
        cancellationToken);

      // update ownerId for warehourse
      if (user.RegisteredRole == RoleConstant.Premises)
      {
        var warehouse = await _context.Warehouses.FindAsync(warehouseId);
        if (warehouse != null && warehouse.OwnerId == null)
        {
          warehouse.OwnerId = user.Id;
          await _context.SaveChangesAsync();
        }
      }
    }
  }

  private string BuildEmailInviteContent(string link, string logo, string inviterName, string locationName, string? roleName)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "InviteUser.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);
    emailContent = emailContent.Replace("{{link}}", link);
    emailContent = emailContent.Replace("{{logo}}", logo);
    emailContent = emailContent.Replace("{{inviter_name}}", inviterName);
    emailContent = emailContent.Replace("{{location_name}}", locationName);
    emailContent = emailContent.Replace("{{role_name}}", roleName);
    emailContent = emailContent.Replace("{{currentYear}}", DateTime.UtcNow.Year.ToString());

    return emailContent;
  }

  private string BuildEmailInviteExistedUserContent(string logo, string inviterName, string invitedName, string locationName, string? roleName)
  {
    var templatePath = Path.Combine("wwwroot", "EmailTemplates", "InviteUserHasExisted.html");
    var emailContent = System.IO.File.ReadAllText(templatePath);

    emailContent = emailContent.Replace("{{logo}}", logo);
    emailContent = emailContent.Replace("{{inviter_name}}", inviterName);
    emailContent = emailContent.Replace("{{invited_name}}", invitedName);
    emailContent = emailContent.Replace("{{location_name}}", locationName);
    emailContent = emailContent.Replace("{{role_name}}", roleName);
    emailContent = emailContent.Replace("{{currentYear}}", DateTime.UtcNow.Year.ToString());

    return emailContent;
  }

  private async Task<bool> CheckExistingUserInWarehouseAsync(Guid? memberId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.WarehouseId == warehouseId && x.UserId == memberId);
    return result != null;
  }

  private async Task<bool> HandlerUserNotExistsAsync(
    string email,
    Guid warehouseId,
    string inviteRole,
    Guid inviterId,
    string? baseDeepLink,
    string logo,
    string inviterName,
    string companyName,
    string? roleName,
    string? baseUrl,
    CancellationToken ct)
  {
    var existingInvitation = await _invitationsRepository
    .FirstOrDefaultAsync(i => i.Email == email &&
                              i.WarehouseId == warehouseId,
                              ct);

    if (existingInvitation == null)
    {
      await _invitationsRepository
        .AddAsync(new Invitation()
        {
          Role = inviteRole,
          Email = email,
          InviteById = inviterId,
          WarehouseId = warehouseId,
        }, ct);
    }
    var linkRedirect = $"{baseUrl}EmailTemplates/RedirectApp.html?deeplink=";
    var deepLink = $"{baseDeepLink}email={email}&inviteRole={inviteRole}&warehouseId={warehouseId}";
    var encodeUrl = Uri.EscapeDataString(deepLink);
    var linkUrl = $"{linkRedirect}{encodeUrl}";
    var content = BuildEmailInviteContent(linkUrl, logo, inviterName, companyName, roleName);
    return await _emailSender.SendAsync(email, EmailConstants.InviteSubject, content);
  }

  private async Task<Result<bool>> HandlerUserExistsAsync(
    string email,
    Guid warehouseId,
    string inviteRole,
    ApplicationUser invitedUser,
    IdentityRole<Guid> role,
    string logo,
    string inviterName,
    string companyName,
    CancellationToken cancellationToken)
  {
    if (await CheckExistingUserInWarehouseAsync(invitedUser.Id, warehouseId))
      return Result.Invalid(new ValidationError("The user already exists in the warehouse."));

    var isRestrictedRole = invitedUser.RegisteredRole is RoleConstant.Premises or RoleConstant.Manager or RoleConstant.Editor;

    if (isRestrictedRole)
      return Result.Invalid(new ValidationError("The user is a premise from another warehouse."));

    var roleMismatch = (inviteRole, invitedUser.RegisteredRole) switch
    {
      (RoleConstant.Inspector, not RoleConstant.Inspector) => "The user is not an inspector.",
      (RoleConstant.Repairer, not RoleConstant.Repairer) => "The user is not a repairer.",
      _ => null
    };

    if (roleMismatch != null)
      return Result.Invalid(new ValidationError(roleMismatch));

    var existingUserRole = await _userWarehouseRoleRepository
      .FirstOrDefaultAsync(
        uwr => uwr.UserId == invitedUser.Id && uwr.WarehouseId == warehouseId,
        cancellationToken);

    if (existingUserRole is null)
    {
      await _userWarehouseRoleRepository.AddAsync(
        new UserWarehouseRole
        {
          UserId = invitedUser.Id,
          WarehouseId = warehouseId,
          RoleId = role.Id,
        },
        cancellationToken);
    }

    string invitedName = $"{invitedUser.FirstName} {invitedUser.LastName}";

    var contentEmail = BuildEmailInviteExistedUserContent(logo, inviterName, invitedName, companyName, role?.Name);
    return await _emailSender.SendAsync(email, EmailConstants.InviteExistedUserSubject, contentEmail);
  }
}
