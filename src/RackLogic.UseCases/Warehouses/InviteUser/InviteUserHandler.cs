using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Common.Constants;
using RackLogic.UseCases.Users;

namespace RackLogic.UseCases.Warehouses.InviteUser;

public class InviteUserHandler(
  IRepositoryWithExpression<Warehouse> _warehouseRepository,
  IRepositoryWithExpression<Invitation> _invitationsRepository,
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IUserWarehouseService _userWarehouseService,
  RoleManager<IdentityRole<Guid>> _roleManager,
  UserManager<ApplicationUser> _userManager,
  IConfiguration _configuration,
  IEmailSender _emailSender,
  IUser _user)
  : ICommandHandler<InviteUserCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(InviteUserCommand request, CancellationToken cancellationToken)
  {
    if (_user.Id is null) return Result.Unauthorized();

    if (!_userWarehouseService.HasPermission(PermissionConstant.ManageUser.Code, PermissionConstant.ManageUser.Create))
      return Result.Forbidden(ErrorMessages.ManageUser.ErrorMessageCreate);

    var inviterId = _user.Id!.Value;

    var role = await _roleManager.FindByNameAsync(request.Role);
    if (role is null) return Result.Invalid(new ValidationError("Role invalid."));

    var result = await _warehouseRepository.FirstOrDefaultAsync(x => x.Id == request.WarehouseId);
    if (result is null) return Result.Invalid(new ValidationError("Warehouse not found."));

    var invitedUser = await _userManager.FindByEmailAsync(request.Email);
    var inviterName = _user.UserName ?? "Someone";
    var deepLink = _configuration.GetSection("Dashboard:InviteUser").Value;
    var baseUrl = _configuration.GetSection("Client:BaseUrl").Value;
    var logo = $"{baseUrl}{_configuration.GetSection("Client:LogoMarket").Value}";

    if (invitedUser is null)
      return await HandlerUserNotExistsAsync(
        request,
        inviterId,
        deepLink,
        logo,
        inviterName,
        result.CompanyName,
        role?.Name,
        baseUrl,
        cancellationToken);

    return await HandlerUserExistsAsync(
      request,
      invitedUser,
      role,
      logo,
      inviterName,
      result.CompanyName,
      cancellationToken);
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
    InviteUserCommand request,
    Guid inviterId,
    string? deepLink,
    string logo,
    string inviterName,
    string companyName,
    string? roleName,
    string? baseUrl,
    CancellationToken ct)
  {
    var existingInvitation = await _invitationsRepository
    .FirstOrDefaultAsync(i => i.Email == request.Email &&
                              i.WarehouseId == request.WarehouseId,
                              ct);

    if (existingInvitation == null)
    {
      await _invitationsRepository
        .AddAsync(new Invitation()
        {
          Role = request.Role,
          Email = request.Email,
          InviteById = inviterId,
          WarehouseId = request.WarehouseId,
        }, ct);
    }
    var linkRedirect = $"{baseUrl}EmailTemplates/RedirectApp.html?deeplink=";
    var linkUrl = $"{linkRedirect}{deepLink}email={request.Email},inviterole={request.Role},warehouseId={request.WarehouseId}";
    var content = BuildEmailInviteContent(linkUrl, logo, inviterName, companyName, roleName);
    return await _emailSender.SendAsync(request.Email, EmailConstants.InviteSubject, content);
  }

  private async Task<Result<bool>> HandlerUserExistsAsync(
    InviteUserCommand request,
    ApplicationUser invitedUser,
    IdentityRole<Guid> role,
    string logo,
    string inviterName,
    string companyName,
    CancellationToken cancellationToken)
  {
    if (await CheckExistingUserInWarehouseAsync(invitedUser.Id, request.WarehouseId))
      return Result.Invalid(new ValidationError("The user already exists in the warehouse."));

    var isRestrictedRole = invitedUser.RegisteredRole is RoleConstant.Premises or RoleConstant.Manager or RoleConstant.Editor;

    if (isRestrictedRole)
      return Result.Invalid(new ValidationError("The user is a premise from another warehouse."));

    var roleMismatch = (request.Role, invitedUser.RegisteredRole) switch
    {
      (RoleConstant.Inspector, not RoleConstant.Inspector) => "The user is not an inspector.",
      (RoleConstant.Repairer, not RoleConstant.Repairer) => "The user is not a repairer.",
      _ => null
    };

    if (roleMismatch != null)
      return Result.Invalid(new ValidationError(roleMismatch));

    var existingUserRole = await _userWarehouseRoleRepository
      .FirstOrDefaultAsync(
        uwr => uwr.UserId == invitedUser.Id && uwr.WarehouseId == request.WarehouseId,
        cancellationToken);

    if (existingUserRole is null)
    {
      await _userWarehouseRoleRepository.AddAsync(
        new UserWarehouseRole
        {
          UserId = invitedUser.Id,
          WarehouseId = request.WarehouseId,
          RoleId = role.Id,
        },
        cancellationToken);
    }

    string invitedName = $"{invitedUser.FirstName} {invitedUser.LastName}";

    var contentEmail = BuildEmailInviteExistedUserContent(logo, inviterName, invitedName, companyName, role?.Name);
    return await _emailSender.SendAsync(request.Email, EmailConstants.InviteExistedUserSubject, contentEmail);
  }
}
