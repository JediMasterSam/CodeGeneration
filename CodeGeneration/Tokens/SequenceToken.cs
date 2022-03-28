using System;
using System.Collections.Generic;
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
        private static Token List(IReadOnlyList<Token> tokens) => new SequenceToken(tokens, ", ");
        private static Token List(params Token[] tokens) => List((IReadOnlyList<Token>)tokens);
        private static Token Sentence(IReadOnlyList<Token> tokens) => new SequenceToken(tokens, " ");
        private static Token Sentence(params Token[] tokens) => Sentence((IReadOnlyList<Token>)tokens);
        private static Token Sequence(params Token[] tokens) => new SequenceToken(tokens);
        private static Token Statements(params Token[] tokens) => new SequenceToken(tokens, "; ");

        private sealed class SequenceToken : Token
        {
            public SequenceToken(IReadOnlyList<Token> tokens, string separator = null)
            {
                if (tokens == null || tokens.Any(token => ReferenceEquals(token, null))) throw new ArgumentNullException(nameof(tokens));
                if (separator == null) separator = string.Empty;

                Tokens = tokens;
                Separator = separator;

                if (tokens.Count == 0)
                {
                    IsEmpty = true;
                    Size = 0;
                }
                else
                {
                    var size = GetSize(tokens, separator.Length);

                    IsEmpty = size == 0;
                    Size = size;
                }
            }

            protected override bool IsEmpty { get; }

            protected override int Size { get; }

            private string Separator { get; }

            private IReadOnlyList<Token> Tokens { get; }

            internal override void AppendTo(StringBuilder stringBuilder)
            {
                if (!IsEmpty)
                {
                    AppendTo(stringBuilder, Tokens, Separator);
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