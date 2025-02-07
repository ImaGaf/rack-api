using System.Security.Claims;
using Ardalis.Result;

namespace RackLogic.UseCases.Users.Delete;
public record DeleteUserCommand(
    ClaimsPrincipal User,
    string DeleteAccountToken) 
    : Ardalis.SharedKernel.ICommand<Result<bool>>;
