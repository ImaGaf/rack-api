using RackLogic.Core.Common.Entity;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.Inspections;
using RackLogic.UseCases.Users.UpdateInspectorProfile;
using Riok.Mapperly.Abstractions;

namespace RackLogic.UseCases.Users;
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class UserUseCaseMapper
{
  public static void UpdateInspectorWithNullCheck(this InspectorProfile existingProfile, UpdateInspectorProfileCommand request)
  {
    existingProfile.CompanyName = request.CompanyName ?? existingProfile.CompanyName;
    existingProfile.ABN = request.ABN ?? existingProfile.ABN;
    existingProfile.CompanyAddress = request.CompanyAddress ?? existingProfile.CompanyAddress;
    existingProfile.CompanyMainPhone = request.CompanyMainPhone ?? existingProfile.CompanyMainPhone;
    existingProfile.InspectorFirstName = request.InspectorFirstName ?? existingProfile.InspectorFirstName;
    existingProfile.InspectorLastName = request.InspectorLastName ?? existingProfile.InspectorLastName;
    existingProfile.InspectorEmailAddress = request.InspectorEmailAddress ?? existingProfile.InspectorEmailAddress;
    existingProfile.InspectorPhoneNumber = request.InspectorPhoneNumber ?? existingProfile.InspectorPhoneNumber;
  }

  [MapperIgnoreSource(nameof(UpdateInspectorProfileCommand.FileImageInspector))]
  [MapperIgnoreTarget(nameof(InspectorProfile.User))]
  [MapperIgnoreTarget(nameof(InspectorProfile.FileImageName))]
  [MapperIgnoreTarget(nameof(InspectorProfile.FileImageUrl))]
  [MapperIgnoreTarget(nameof(InspectorProfile.IsDeleted))]
  [MapperIgnoreTarget(nameof(InspectorProfile.DeleterId))]
  [MapperIgnoreTarget(nameof(InspectorProfile.DeletionTime))]
  [MapperIgnoreTarget(nameof(InspectorProfile.LastModificationTime))]
  [MapperIgnoreTarget(nameof(InspectorProfile.LastModifierId))]
  [MapperIgnoreTarget(nameof(InspectorProfile.CreationTime))]
  [MapperIgnoreTarget(nameof(InspectorProfile.CreatorId))]
  [MapperIgnoreTarget(nameof(InspectorProfile.Id))]
  public static partial InspectorProfile ToInspectorProfile(this UpdateInspectorProfileCommand request, Guid userId);



  [MapperIgnoreTarget(nameof(InspectorProfileDTO.CompanyName))]
  [MapperIgnoreTarget(nameof(InspectorProfileDTO.ABN))]
  [MapperIgnoreTarget(nameof(InspectorProfileDTO.CompanyAddress))]
  [MapperIgnoreTarget(nameof(InspectorProfileDTO.CompanyMainPhone))]
  [MapperIgnoreTarget(nameof(InspectorProfileDTO.FileImagePath))]
  [MapProperty(nameof(ApplicationUser.Id), nameof(InspectorProfileDTO.UserId))]
  [MapProperty(nameof(ApplicationUser.FirstName), nameof(InspectorProfileDTO.InspectorFirstName))]
  [MapProperty(nameof(ApplicationUser.LastName), nameof(InspectorProfileDTO.InspectorLastName))]
  [MapProperty(nameof(ApplicationUser.PhoneNumber), nameof(InspectorProfileDTO.InspectorPhoneNumber))]
  [MapProperty(nameof(ApplicationUser.Email), nameof(InspectorProfileDTO.InspectorEmailAddress))]
  public static partial InspectorProfileDTO UserToInspectorDTO(this ApplicationUser user);

  public static InspectorProfileDTO ToInspectorProfileDTO(this InspectorProfile profile)
  {
    return new InspectorProfileDTO
    {
      UserId = profile.UserId,
      CompanyName = profile.CompanyName,
      ABN = profile.ABN,
      CompanyAddress = profile.CompanyAddress,
      CompanyMainPhone = profile.CompanyMainPhone,
      FileImagePath = profile.FileImageUrl!,
      InspectorFirstName = profile.InspectorFirstName,
      InspectorLastName = profile.InspectorLastName,
      InspectorPhoneNumber = profile.InspectorPhoneNumber,
      InspectorEmailAddress = profile.InspectorEmailAddress
    };
  }
}
