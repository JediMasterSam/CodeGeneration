using System;
using System.Linq;
using System.Text;

namespace CodeGeneration
{
    public partial class Token
    {
        private static Token Braces(Token token) => Sequence(Literal("{ "), token, Literal(" }"));
        private static Token Brackets(Token token) => Sequence(Literal("["), token, Literal("]"));
        private static Token Label(Token token) => Sequence(token, Literal(":"));
        private static Token Parentheses(Token token) => Sequence(Literal("("), token, Literal(")"));
        private static Token Statement(Token token) => Sequence(token, Literal(";"));

        private static Token Lines(params Token[] tokens) => new SequenceToken(tokens, "\n");
        private static Token List(params Token[] tokens) => new SequenceToken(tokens, ", ");
        private static Token Sentence(params Token[] tokens) => new SequenceToken(tokens, " ");
        private static Token Sequence(params Token[] tokens) => new SequenceToken(tokens);
        private static Token Statements(params Token[] tokens) => new SequenceToken(tokens, "; ");

        private sealed class SequenceToken : Token
        {
            public SequenceToken(Token[] tokens, string separator = null)
            {
                if (tokens == null || tokens.Any(token => ReferenceEquals(token, null))) throw new ArgumentNullException(nameof(tokens));

                Tokens = tokens;
                Separator = separator ?? string.Empty;
            }

            private string Separator { get; }

            private Token[] Tokens { get; }

            internal override void AppendTo(StringBuilder stringBuilder)
            {
                var appendSeparator = false;

                foreach (var token in Tokens)
                {
                    if (appendSeparator)
                    {
                        stringBuilder.Append(Separator);
                    }
                    else
                    {
                        appendSeparator = true;
                    }

                    token.AppendTo(stringBuilder);
                }
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is SequenceToken other && Separator.Equals(other.Separator) && Tokens.SequenceEqual(other.Tokens);
            }

            public override int GetHashCode()
            {
                return Tuple.Create(Separator, Tokens).GetHashCode();
            }
        }
    }
}