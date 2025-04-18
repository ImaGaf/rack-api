using System.Security.Claims;
using Ardalis.Result;

namespace RackLogic.UseCases.Auths.Logout;
public record LogoutCommand()
  : Ardalis.SharedKernel.ICommand<Result>;
