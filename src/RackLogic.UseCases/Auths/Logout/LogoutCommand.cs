using System.Security.Claims;
using Ardalis.Result;

namespace RackLogic.UseCases.Auths.Logout;
public record LogoutCommand(ClaimsPrincipal user) : Ardalis.SharedKernel.ICommand<Result>;
