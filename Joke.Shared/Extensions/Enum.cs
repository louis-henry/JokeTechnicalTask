using System.ComponentModel;
using System.Reflection;

namespace Joke.Shared.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Get the Description from the DescriptionAttribute.
    /// </summary>
    /// <returns></returns>
    public static string GetDescription(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DescriptionAttribute>()?
            .Description ?? enumValue.ToString();
    }
}