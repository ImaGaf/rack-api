using Ardalis.Result;

namespace RackLogic.UseCases.Users.ResetPassword;
public record ResetPasswordCommand(string Email, string NewPassword) : Ardalis.SharedKernel.ICommand<Result<bool>>;

