using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace RackLogic.UseCases.Users.UpdateInspectorProfile;
public record UpdateInspectorProfileCommand(
  string? UserRegisterRole,
  string? CompanyName,
  string? ABN,
  string? CompanyAddress,
  string? CompanyMainPhone,
  IFormFile? FileImageInspector,
  string? InspectorFirstName,
  string? InspectorLastName,
  string? InspectorPhoneNumber,
  string? InspectorEmailAddress)
  : ICommand<Result<bool>>;

