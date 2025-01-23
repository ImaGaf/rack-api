using Ardalis.Result;

namespace RackLogic.UseCases.Users.ConfirmPassword;
public record ConfirmPasswordCommand(
    string Email,
    string OldPassword)
    : Ardalis.SharedKernel.ICommand<Result<string>>;
