using System;

namespace TwoHundred.Server.Extensions;

public static class EnumExtensions
{
    public static string ToString(this Enum eff)
    {
        return Enum.GetName(eff.GetType(), eff) ?? string.Empty;
    }

    public static EnumType ToEnum<EnumType>(this string enumValue)
    {
        return (EnumType)Enum.Parse(typeof(EnumType), enumValue);
    }
}
