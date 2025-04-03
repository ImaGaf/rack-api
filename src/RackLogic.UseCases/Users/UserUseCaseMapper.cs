using RackLogic.Core.Common.Entity;
using RackLogic.UseCases.Users.UpdateInspectorProfile;

namespace RackLogic.UseCases.Users;
public static partial class UserUseCaseMapper
{
  public static void UpdateInspectorProfileFromRequest(this InspectorProfile existingProfile, UpdateInspectorProfileCommand request)
  {
    existingProfile.Companyname = request.CompanyName;
    existingProfile.ABN = request.ABN;
    existingProfile.CompanyAddress = request.CompanyAddress;
    existingProfile.CompanyMainPhone = request.CompanyMainPhone;
    existingProfile.InspectorFirstName = request.InspectorFirstName;
    existingProfile.InspectorLastName = request.InspectorLastName;
    existingProfile.InspectorEmailAddress = request.InspectorEmailAddress;
    existingProfile.InspectorPhoneNumber = request.InspectorPhoneNumber;
  }

  public static InspectorProfile ToInspectorProfile(this UpdateInspectorProfileCommand request, Guid creatorId)
  {
    return new InspectorProfile
    {
      UserId = creatorId,
      Companyname = request.CompanyName,
      ABN = request.ABN,
      CompanyAddress = request.CompanyAddress,
      CompanyMainPhone = request.CompanyMainPhone,
      InspectorFirstName = request.InspectorFirstName,
      InspectorLastName = request.InspectorLastName,
      InspectorEmailAddress = request.InspectorEmailAddress,
      InspectorPhoneNumber = request.InspectorPhoneNumber
    };
  }

  public static InspectorProfileDTO ToInspectorProfileDTO(this InspectorProfile profile)
  {
    return new InspectorProfileDTO
    {
      UserId = profile.UserId,
      CompanyName = profile.Companyname,
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
