using System.Linq;
using System.Text;

namespace CodeGeneration
{
    public static class StringExtensions
    {
        public static string ToPrintable(this string value)
        {
            if (value == null) return "null";
            if (value.All(character => character.IsPrintable())) return $"\"{value}\"";

            var stringBuilder = new StringBuilder(value.Length + 2).Append('\"');

            for (var index = 0; index < value.Length; index++)
            {
                stringBuilder.Append(value[index].ToPrintable());
            }

            return stringBuilder.Append('\"').ToString();
        }
    }
}