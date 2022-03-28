using System;
using System.Text;

namespace CodeGeneration
{
    public abstract partial class Token
    {
        private bool _isLogical;

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
            return new StringBuilder().AppendToken(this).ToString();
        }

        internal abstract void AppendTo(StringBuilder stringBuilder);
    }
}