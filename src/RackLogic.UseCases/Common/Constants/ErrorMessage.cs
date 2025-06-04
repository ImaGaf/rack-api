namespace RackLogic.UseCases.Common.Constants;

public static class ErrorMessages
{
  public static class Account
  {
    public const string ErrorMessageCreate = "You do not have permission to create accounts.";
    public const string ErrorMessageUpdate = "You do not have permission to update accounts.";
    public const string ErrorMessageDelete = "You do not have permission to delete accounts.";
  }

  public static class DefectLog
  {
    public const string ErrorMessageCreate = "You do not have permission to create defect logs.";
    public const string ErrorMessageView = "You do not have permission to view defect logs.";
    public const string ErrorMessageUpdate = "You do not have permission to update defect logs.";
    public const string ErrorMessageDelete = "You do not have permission to delete defect logs.";
  }

  public static class BayConfiguration
  {
    public const string ErrorMessageCreate = "You do not have permission to create bay configurations.";
    public const string ErrorMessageView = "You do not have permission to view bay configurations.";
    public const string ErrorMessageUpdate = "You do not have permission to update bay configurations.";
    public const string ErrorMessageDelete = "You do not have permission to delete bay configurations.";
  }

  public static class FloorPlan
  {
    public const string ErrorMessageCreate = "You do not have permission to create floor plans.";
    public const string ErrorMessageView = "You do not have permission to view floor plans.";
    public const string ErrorMessageUpdate = "You do not have permission to update floor plans.";
    public const string ErrorMessageDelete = "You do not have permission to delete floor plans.";
  }

  public static class Inspection
  {
    public const string ErrorMessageConduct = "You do not have permission to conduct inspections.";
    public const string ErrorMessageAuthorise = "You do not have permission to authorise inspections.";
    public const string ErrorMessageViewSchedule = "You do not have permission to view inspection schedules.";
    public const string ErrorMessageViewReport = "You do not have permission to view inspection reports.";
  }

  public static class UserRole
  {
    public const string ErrorMessageCreate = "You do not have permission to assign user roles.";
    public const string ErrorMessageUpdate = "You do not have permission to update user roles.";
    public const string ErrorMessageDelete = "You do not have permission to remove user roles.";
  }
}
