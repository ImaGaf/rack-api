using Ardalis.Result;

namespace RackLogic.UseCases.Users.Delete;
public record DeleteUserCommand(
    Guid UserId,
    string DeleteAccountToken) 
    : Ardalis.SharedKernel.ICommand<Result<bool>>;
