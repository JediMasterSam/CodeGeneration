using System;
using System.Text;

namespace CodeGeneration
{
    public partial class Token
    {
        private enum OperationType
        {
            Primary,
            Secondary,
            Range,
            Pattern,
            Multiplicative,
            Additive,
            Shift,
            Relational,
            Equality,
            And,
            ExclusiveOr,
            Or,
            AndAlso,
            OrElse,
            Coalesce,
            Conditional,
            Assignment
        }

        private static Token Sentence(OperationType operationType, params Token[] operands) => new OperationToken(operationType, operands, true);
        private static Token Sequence(OperationType operationType, params Token[] operands) => new OperationToken(operationType, operands, false);

        private sealed class OperationToken : Token
        {
            public OperationToken(OperationType operationType, Token[] operands, bool separateOperands)
            {
                if (operands == null) throw new ArgumentNullException(nameof(operands));

                for (var index = 0; index < operands.Length; index++)
                {
                    if (operands[index] is OperationToken other && other.OperationType > operationType)
                    {
                        operands[index] = Parentheses(other);
                    }
                }

                OperationType = operationType;
                Operands = separateOperands ? Sentence(operands) : Sequence(operands);
            }

            protected override bool IsEmpty => Operands.IsEmpty;

            protected override int Size => Operands.Size;

            private OperationType OperationType { get; }

            private Token Operands { get; }

            internal override void AppendTo(StringBuilder stringBuilder)
            {
                Operands.AppendTo(stringBuilder);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(obj, this) || obj is OperationToken other && OperationType.Equals(other.OperationType) && Operands.Equals(other.Operands);
            }

            public override int GetHashCode()
            {
                return Tuple.Create(OperationType, Operands).GetHashCode();
            }
        }
    }
}