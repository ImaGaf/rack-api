using Ardalis.Result;

namespace RackLogic.UseCases.Users.VerifyOtp;
public record VerifyOtpCommand (
  string Email,
  string Otp)
  : Ardalis.SharedKernel.ICommand<Result<string?>>;
