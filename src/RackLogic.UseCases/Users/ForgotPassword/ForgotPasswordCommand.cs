using Ardalis.Result;

namespace RackLogic.UseCases.Users.ForgotPassword;
public record ForgotPasswordCommand(string Email) : Ardalis.SharedKernel.ICommand<Result<bool>>;
