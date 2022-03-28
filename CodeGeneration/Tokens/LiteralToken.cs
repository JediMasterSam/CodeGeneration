using System;
using System.Text;

namespace CodeGeneration
{
    public partial class Token
    {
        public static Token Literal(string value) => new LiteralToken(value);
        public static Token Type(Type type) => new LiteralToken(type.ToPrintable());
        public static Token Type<T>() => Type(typeof(T));

        private sealed class LiteralToken : Token
        {
            public LiteralToken(string value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }

            private string Value { get; }

            internal override void AppendTo(StringBuilder stringBuilder)
            {
                stringBuilder.Append(Value);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is LiteralToken other && Value.Equals(other.Value) || obj is string value && Value.Equals(value);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }

            public override string ToString()
            {
                return Value;
            }
        }
    }
}