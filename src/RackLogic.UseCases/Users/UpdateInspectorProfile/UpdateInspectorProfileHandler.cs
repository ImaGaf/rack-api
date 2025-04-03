using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.FloorPlans;

namespace RackLogic.UseCases.Users.UpdateInspectorProfile;
public class UpdateInspectorProfileHandler(
  UserManager<ApplicationUser> _userManager,
  IFileService _fileService,
  IRepositoryWithExpression<InspectorProfile> _inspectorProfileRepository)
  : ICommandHandler<UpdateInspectorProfileCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(UpdateInspectorProfileCommand request, CancellationToken cancellationToken)
  {
    var creatorId = request.UserCreatedId; 
    var creatorRole = request.UserRegisterRole;

    // Check if User exists
    var user = await _userManager.FindByIdAsync(creatorId.ToString());
    if (user == null) return Result.Invalid(new ValidationError("User not found."));

    // Check if User Register Role is Inspector
    if (creatorRole != RoleConstant.Inspector) return Result.Invalid(new ValidationError("User must have the Inspector role."));

    // Check if Userd already exists in InspectorProfile table
    var existingInspectorProfile = await _inspectorProfileRepository.FirstOrDefaultAsync(i => i.UserId == creatorId, cancellationToken);

    // Path folder
    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "InspectorProfileImage");

    if (existingInspectorProfile != null)
    {
      // Update InspectorProfile
      existingInspectorProfile.UpdateInspectorProfileFromRequest(request);
      await _inspectorProfileRepository.UpdateToContextAsync(existingInspectorProfile);
      await _fileService.DeleteFilesAsyncByUserId(existingInspectorProfile.UserId, rootPath);

      // Call func Update File
      if (!await AddFileAsync(existingInspectorProfile, creatorId, request.FileImageInspector, rootPath, cancellationToken)) return Result.Error("Failed to save Inspector logo file.");
    }

    else
    {
      // Create InspectorProfile 
      var inspectorProfile = request.ToInspectorProfile(creatorId);
      await _inspectorProfileRepository.AddToContextAsync(inspectorProfile);

      // Call func Savefile
      if (!await AddFileAsync(inspectorProfile, creatorId, request.FileImageInspector, rootPath, cancellationToken)) return Result.Error("Failed to save Inspector logo file.");
    }

    return Result.Success(true);
  }

  private async Task<bool> AddFileAsync(InspectorProfile inspectorProfile , Guid inspectorId, IFormFile fileInspectorImage, string rootPath, CancellationToken cancellationToken)
  {
    var savedFiles = await _fileService.SaveFilesAsyncByUserId(inspectorId, new List<IFormFile> { fileInspectorImage }, rootPath);
    var savedFile = savedFiles.FirstOrDefault();

    if (savedFile == null) return false;

    inspectorProfile.FileImageUrl = savedFile.FilePath;
    inspectorProfile.FileImageName = savedFile.FileName;
    await _inspectorProfileRepository.SaveChangesAsync(cancellationToken);

    return true;
  }
}
