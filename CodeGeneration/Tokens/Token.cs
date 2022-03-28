using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneration
{
    public abstract partial class Token
    {
        private bool _isLogical;

        protected abstract bool IsEmpty { get; }

        protected abstract int Size { get; }

        private bool IsLogical
        {
            get
            {
                var isLogical = _isLogical;

                _isLogical = false;
                return isLogical;
            }
            set => _isLogical = value;
        }

        public static bool operator true(Token token)
        {
            token.IsLogical = true;
            return false;
        }

        public static bool operator false(Token token)
        {
            token.IsLogical = true;
            return false;
        }

        public override bool Equals(object obj)
        {
            throw new Exception($"{GetType()} has no implementation defined for {nameof(Equals)}.");
        }

        public override int GetHashCode()
        {
            throw new Exception($"{GetType()} has no implementation defined for {nameof(GetHashCode)}.");
        }

        public override string ToString()
        {
            return new StringBuilder(Size).AppendToken(this).ToString();
        }

        internal abstract void AppendTo(StringBuilder stringBuilder);

        private static void AppendTo(StringBuilder stringBuilder, IEnumerable<Token> tokens, string separator)
        {
            var appendSeparator = false;

            foreach (var token in tokens)
            {
                if (token.IsEmpty) continue;

                if (appendSeparator)
                {
                    stringBuilder.Append(separator);
                }
                else
                {
                    appendSeparator = true;
                }

                token.AppendTo(stringBuilder);
            }
        }

        private static int GetSize(IEnumerable<Token> tokens, int separatorSize)
        {
            var size = 0;
            var count = 0;

            foreach (var token in tokens)
            {
                if (token.IsEmpty) continue;
                size += token.Size;
                count++;
            }

            return count == 0 ? 0 : size + (count - 1) * separatorSize;
        }
    }
}