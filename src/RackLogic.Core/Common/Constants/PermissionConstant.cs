namespace RackLogic.Core.Common.Constants;

public static class PermissionConstant
{
  public static class Account
  {
    public const string Code = "account";

    public const string FullThemselves = "account.full-themselves";
    public const string FullEveryone = "account.full-everyone";

    public const string CreateThemselves = "account.create-themselves";
    public const string UpdateThemselves = "account.update-themselves";
    public const string DeleteThemselves = "account.delete-themselves";

    public const string CreateEveryone = "account.create-everyone";
    public const string UpdateEveryone = "account.update-everyone";
    public const string DeleteEveryone = "account.delete-everyone";
  }

  public static class UserRole
  {
    public const string Code = "user-role";

    public const string Full = "user-role.full";

    public const string CreateManager = "user-role.create-manager";
    public const string CreateInspector = "user-role.create-inspector";
    public const string CreateEditor = "user-role.create-editor";
    public const string CreateRepairer = "user-role.create-repairer";

    public const string UpdateManager = "user-role.update-manager";
    public const string UpdateInspector = "user-role.update-inspector";
    public const string UpdateEditor = "user-role.update-editor";
    public const string UpdateRepairer = "user-role.update-repairer";

    public const string DeleteManager = "user-role.delete-manager";
    public const string DeleteInspector = "user-role.delete-inspector";
    public const string DeleteEditor = "user-role.delete-editor";
    public const string DeleteRepairer = "user-role.delete-repairer";
  }

  public static class DefectLog
  {
    public const string Code = "defect-log";

    public const string Full = "defect-log.full";

    public const string Create = "defect-log.create";
    public const string View = "defect-log.view";
    public const string Resolve = "defect-log.resolve";
    public const string UpdateStatus = "defect-log.update-status";
    public const string AuthoriseResolve = "defect-log.authorise-resolve";
  }

  public static class Inspection
  {
    public const string Code = "inspection";

    public const string Full = "inspection.full";

    public const string Conduct = "inspection.conduct";
    public const string Authorise = "inspection.authorise";
    public const string ViewSchedule = "inspection.view-schedule";
    public const string ViewReport = "inspection.view-report";
  }

  public static class FloorPlan
  {
    public const string Code = "floorplan";

    public const string Full = "floorplan.full";

    public const string Create = "floorplan.create";
    public const string View = "floorplan.view";
    public const string Update = "floorplan.update";
    public const string Delete = "floorplan.delete";
  }

  public static class BayConfiguration
  {
    public const string Code = "bay-configuration";

    public const string Full = "bay-configuration.full";

    public const string Create = "bay-configuration.create";
    public const string View = "bay-configuration.view";
    public const string Update = "bay-configuration.update";
    public const string Delete = "bay-configuration.delete";
  }
}
