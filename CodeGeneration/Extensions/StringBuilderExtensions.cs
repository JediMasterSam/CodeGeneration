using System.Text;

namespace CodeGeneration
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendToken(this StringBuilder stringBuilder, Token token)
        {
            token.AppendTo(stringBuilder);
            return stringBuilder;
        }
    }
}