using System.Collections.Generic;

namespace CodeGeneration
{
    public static class CharExtensions
    {
        private static readonly IReadOnlyDictionary<char, string> EscapedSequences = new Dictionary<char, string>
        {
            { '\a', "\\a" },
            { '\b', "\\b" },
            { '\f', "\\f" },
            { '\n', "\\n" },
            { '\r', "\\r" },
            { '\t', "\\t" },
            { '\v', "\\v" },
            { '\'', "\\\'" },
            { '\"', "\\\"" },
            { '\\', "\\\\" }
        };

        public static bool IsPrintable(this char value)
        {
            return !char.IsControl(value) && !EscapedSequences.ContainsKey(value);
        }

        public static string ToPrintable(this char value)
        {
            return EscapedSequences.TryGetValue(value, out var sequence) ? sequence : char.IsControl(value) ? $"\\u{(int)value: X4}" : $"{value}";
        }
    }
}