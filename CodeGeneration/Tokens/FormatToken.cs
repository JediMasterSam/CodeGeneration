using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGeneration
{
    public partial class Token
    {
        public static Token Block(params Token[] body) => new BlockToken(null, null, body);

        private static Token Block(Token openingStatement, Token[] body) => new BlockToken(openingStatement, null, body);
        private static Token Block(string name, Token parameter, Token[] body) => Block(Sentence(Literal(name), Parentheses(parameter)), body);
        private static Token Block(Token openingStatement, Token closingStatement, Token[] body) => new BlockToken(openingStatement, closingStatement, body);

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

        private sealed class BlockToken : FormatToken
        {
            public BlockToken(Token openingStatement, Token closingStatement, IReadOnlyList<Token> body)
            {
                if (body == null) throw new ArgumentNullException(nameof(body));

                var hasOpeningStatement = !ReferenceEquals(openingStatement, null);
                var offset = hasOpeningStatement ? 2 : 1;
                var buffer = new FormatToken[body.Count + 2 + (hasOpeningStatement ? 1 : 0)];

                if (hasOpeningStatement)
                {
                    buffer[0] = new LineToken(openingStatement);
                    buffer[1] = new LineToken(Literal("{"));
                }
                else
                {
                    buffer[0] = new LineToken(Literal("{"));
                }

                if (!ReferenceEquals(closingStatement, null))
                {
                    buffer[buffer.Length - 1] = new LineToken(Sentence(Literal("}"), closingStatement));
                }
                else
                {
                    buffer[buffer.Length - 1] = new LineToken(Literal("}"));
                }

                for (var index = 0; index < body.Count; index++)
                {
                    switch (body[index])
                    {
                        case FormatToken other:
                        {
                            buffer[index + offset] = other.Indent();
                            break;
                        }
                        case OperationToken operation:
                        {
                            buffer[index + offset] = new LineToken(Statement(operation)).Indent();
                            break;
                        }
                        default:
                        {
                            buffer[index + offset] = new LineToken(body[index]).Indent();
                            break;
                        }
                    }
                }

                Body = buffer;
                Size = GetSize(buffer);
            }

            private BlockToken(IReadOnlyList<FormatToken> body)
            {
                Body = body ?? throw new ArgumentNullException(nameof(body));
                Size = GetSize(body);
            }

            protected override bool IsEmpty => false;

            protected override int Size { get; }

            private IReadOnlyList<FormatToken> Body { get; }

            internal override void AppendTo(StringBuilder stringBuilder)
            {
                AppendTo(stringBuilder, Body, "\n");
            }

            public override FormatToken Indent()
            {
                var body = new FormatToken[Body.Count];

                for (var index = 0; index < body.Length; index++)
                {
                    body[index] = Body[index].Indent();
                }

                return new BlockToken(body);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is BlockToken other && Body.SequenceEqual(other.Body);
            }

            public override int GetHashCode()
            {
                return Body.GetHashCode();
            }

            private static int GetSize(IEnumerable<FormatToken> body)
            {
                return GetSize(body, 1);
            }
        }
    }
}