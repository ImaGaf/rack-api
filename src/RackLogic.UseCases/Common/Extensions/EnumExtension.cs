using System.ComponentModel.DataAnnotations;

namespace RackLogic.UseCases.Common.Extensions;

public static class EnumExtensions
{
  public static string GetDisplayName(this Enum enumValue)
  {
    var displayAttribute = GetDisplayAttribute(enumValue);
    return displayAttribute?.Name ?? enumValue.ToString();
  }

  public static string GetDescription(this Enum enumValue)
  {
    var displayAttribute = GetDisplayAttribute(enumValue);
    return displayAttribute?.Description ?? enumValue.ToString();
  }

  private static DisplayAttribute? GetDisplayAttribute(Enum enumValue)
  {
    return enumValue.GetType()
      .GetMember(enumValue.ToString())
      .FirstOrDefault()?
      .GetCustomAttributes(typeof(DisplayAttribute), false)
      .FirstOrDefault() as DisplayAttribute;
  }
}
