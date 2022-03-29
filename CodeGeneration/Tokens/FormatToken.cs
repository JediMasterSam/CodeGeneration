using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGeneration
{
    public partial class Token
    {
        public static Token Block(params Token[] tokens) => Block(Empty(), Empty(), tokens);
        public static Token Group(params Token[] tokens) => new GroupToken(tokens);
        
        private static Token Block(Token openingStatement, IReadOnlyList<Token> body) => Block(openingStatement, Empty(), body); 
        private static Token Block(string name, Token parameter, IReadOnlyList<Token> body) => Block(Sentence(Literal(name), Parentheses(parameter)), body);
        private static Token Block(Token openingStatement, Token closingStatement, IReadOnlyList<Token> tokens) => Group(openingStatement, Literal("{"), new GroupToken(tokens).Indent(), Sentence(Literal("}"), closingStatement));

        private abstract class FormatToken : Token
        {
            public abstract FormatToken Indent();
        }

        private sealed class LineToken : FormatToken
        {
            public LineToken(Token token, int offset = 0)
            {
                Token = token ?? throw new ArgumentNullException(nameof(token));
                Offset = offset;
                Size = token.IsEmpty ? 0 : offset + token.Size;
            }

            protected override bool IsEmpty => Token.IsEmpty;

            protected override int Size { get; }

            private Token Token { get; }

            private int Offset { get; }

            internal override void AppendTo(StringBuilder stringBuilder)
            {
                if (!IsEmpty)
                {
                    stringBuilder.Append(' ', Offset).AppendToken(Token);
                }
            }

            public override FormatToken Indent()
            {
                return new LineToken(Token, Offset + 4);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is LineToken other && Offset.Equals(other.Offset) && Token.Equals(other.Token);
            }

            public override int GetHashCode()
            {
                return Tuple.Create(Offset, Token).GetHashCode();
            }
        }

        private sealed class GroupToken : FormatToken
        {
            public GroupToken(IReadOnlyList<Token> tokens)
            {
                if (tokens == null) throw new ArgumentNullException(nameof(tokens));

                var buffer = new FormatToken[tokens.Count];

                for (var index = 0; index < buffer.Length; index++)
                {
                    buffer[index] = tokens[index] is FormatToken token ? token : new LineToken(tokens[index]);
                }

                var size = GetSize(buffer);

                IsEmpty = size == 0;
                Size = size;
                Tokens = buffer;
            }

            private GroupToken(IReadOnlyList<FormatToken> tokens)
            {
                var size = GetSize(tokens);

                IsEmpty = size == 0;
                Size = size;
                Tokens = tokens;
            }

            protected override bool IsEmpty { get; }

            protected override int Size { get; }

            private IReadOnlyList<FormatToken> Tokens { get; }

            internal override void AppendTo(StringBuilder stringBuilder)
            {
                AppendTo(stringBuilder, Tokens, "\n");
            }

            public override FormatToken Indent()
            {
                var tokens = new FormatToken[Tokens.Count];

                for (var index = 0; index < tokens.Length; index++)
                {
                    tokens[index] = Tokens[index].Indent();
                }

                return new GroupToken(tokens);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is GroupToken other && Tokens.SequenceEqual(other.Tokens);
            }

            public override int GetHashCode()
            {
                return Tokens.GetHashCode();
            }

            private static int GetSize(IEnumerable<FormatToken> body)
            {
                return GetSize(body, 1);
            }
        }
    }
}