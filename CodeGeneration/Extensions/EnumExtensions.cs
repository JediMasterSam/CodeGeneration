using System;
using System.Linq;

namespace CodeGeneration
{
    public static class EnumExtensions
    {
        public static string ToPrintable(this Enum value)
        {
            var type = value.GetType();
            var name = type.ToPrintable();

            return type.IsDefined(typeof(FlagsAttribute), false)
                ? string.Join(" | ", Enum.GetValues(type).Cast<Enum>().Where(value.HasFlag).Select(flag => $"{name}.{flag}"))
                : $"{name}.{value}";
        }
    }
}