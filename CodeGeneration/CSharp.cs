using System;
using System.Collections.Generic;

namespace CodeGeneration
{
    public abstract partial class Token
    {
        #region Types

        public static Token Bool() => Literal("bool");
        public static Token Byte() => Literal("byte");
        public static Token Char() => Literal("char");
        public static Token Decimal() => Literal("decimal");
        public static Token Delegate() => Literal("delegate");
        public static Token Double() => Literal("double");
        public static Token Dynamic() => Literal("dynamic");
        public static Token Enum() => Literal("enum");
        public static Token Event() => Literal("event");
        public static Token Float() => Literal("float");
        public static Token Int() => Literal("int");
        public static Token Long() => Literal("long");
        public static Token NInt() => Literal("nint");
        public static Token NuInt() => Literal("nuint");
        public static Token Object() => Literal("object");
        public static Token SByte() => Literal("sbyte");
        public static Token Short() => Literal("short");
        public static Token String() => Literal("string");
        public static Token UInt() => Literal("uint");
        public static Token ULong() => Literal("ulong");
        public static Token UShort() => Literal("ushort");
        public static Token Void() => Literal("void");

        #endregion

        #region Values

        public static Token Base() => Literal("base");
        public static Token Default() => Literal("default");
        public static Token False() => Literal("false");
        public static Token Null() => Literal("null");
        public static Token This() => Literal("this");
        public static Token True() => Literal("true");

        #endregion

        #region Accessibiliy

        public static Token Internal() => Literal("internal");
        public static Token Private() => Literal("private");
        public static Token PrivateProtected() => Literal("protected private");
        public static Token Protected() => Literal("protected");
        public static Token ProtectedInternal() => Literal("protected internal");
        public static Token Public() => Literal("public");

        #endregion

        #region Modifiers

        public static Token Abstract() => Literal("abstract");
        public static Token Const() => Literal("const");
        public static Token Explicit() => Literal("explicit");
        public static Token Extern() => Literal("extern");
        public static Token Implicit() => Literal("implicit");
        public static Token New() => Literal("new");
        public static Token Operator() => Literal("operator");
        public static Token Override() => Literal("override");
        public static Token Partial() => Literal("partial");
        public static Token Readonly() => Literal("readonly");
        public static Token Sealed() => Literal("sealed");
        public static Token Static() => Literal("static");
        public static Token Unsafe() => Literal("unsafe");
        public static Token Virtual() => Literal("virtual");
        public static Token Volatile() => Literal("volatile");

        public static Token Const(Token parameter) => Sentence(Literal("const"), parameter);
        public static Token In(Token parameter) => Sentence(Literal("in"), parameter);
        public static Token Out(Token parameter) => Sentence(Literal("out"), parameter);
        public static Token Params(Token parameter) => Sentence(Literal("params"), parameter);
        public static Token Ref(Token parameter) => Sentence(Literal("ref"), parameter);

        #endregion

        #region Conversions

        public static implicit operator Token(Enum value) => Literal(value.ToPrintable());
        public static implicit operator Token(bool value) => Literal(value ? "true" : "false");
        public static implicit operator Token(byte value) => Literal($"(byte) {value}");
        public static implicit operator Token(char value) => Literal(value.ToPrintable());
        public static implicit operator Token(decimal value) => Literal($"{value}m");
        public static implicit operator Token(double value) => Literal($"{value}d");
        public static implicit operator Token(float value) => Literal($"{value}f");
        public static implicit operator Token(int value) => Literal($"{value}");
        public static implicit operator Token(long value) => Literal($"{value}L");
        public static implicit operator Token(sbyte value) => Literal($"(sbyte) {value}");
        public static implicit operator Token(short value) => Literal($"(short) {value}");
        public static implicit operator Token(string value) => Literal(value.ToPrintable());
        public static implicit operator Token(uint value) => Literal($"{value}u");
        public static implicit operator Token(ulong value) => Literal($"{value}ul");
        public static implicit operator Token(ushort value) => Literal($"(ushort) {value}");

        #endregion

        #region Parameters

        public static Token Parameter(Token type, Token name) => Sentence(type, name);
        public static Token Parameter<T>(Token name) => Parameter(Type<T>(), name);
        public static Token Parameter<T>(string name) => Parameter<T>(Literal(name));

        #endregion

        #region Variables

        public static Token Variable(Token name) => Variable(Literal("var"), name);
        public static Token Variable(Token type, Token name) => Sentence(OperationType.Primary, type, name);
        public static Token Variable<T>(Token name) => Variable(Type<T>(), name);
        public static Token Variable<T>(string name) => Variable<T>(Literal(name));

        #endregion

        #region Jumps

        public static Token Break() => Sequence(OperationType.Primary, Literal("break"));
        public static Token Continue() => Sequence(OperationType.Primary, Literal("continue"));
        public static Token GoTo(Token expression) => Sentence(OperationType.Primary, Literal("goto"), expression);
        public static Token GoTo(string name) => GoTo(Literal(name));
        public static Token New(Token expression) => Sentence(OperationType.Primary, Literal("new"), expression);
        public static Token Return(Token expression) => Sentence(OperationType.Primary, Literal("return"), expression);
        public static Token Throw(Token expression) => Sentence(OperationType.Primary, Literal("throw"), expression);
        public static Token YieldBreak() => Sequence(OperationType.Primary, Literal("yield break"));
        public static Token YieldReturn(Token expression) => Sentence(OperationType.Primary, Literal("yield return"), expression);

        #endregion

        #region Loops

        public static Token DoWhile(Token condition, params Token[] body) => Block(Literal("do"), Sentence(Literal("while"), Parentheses(condition)), body);
        public static Token For(Token initializer, Token condition, Token iterator, params Token[] body) => Block("for", Statements(initializer, condition, iterator), body);
        public static Token ForEach(Token element, Token collection, params Token[] body) => Block("foreach", Sentence(element, Literal("in"), collection), body);
        public static Token While(Token condition, params Token[] body) => Block("while", condition, body);

        #endregion

        #region Selections

        public static Token Else(params Token[] body) => Block(Literal("else"), body);
        public static Token ElseIf(Token condition, params Token[] body) => Block("else if", condition, body);
        public static Token If(Token condition, params Token[] body) => Block("if", condition, body);
        public static Token Switch(Token parameter, params Token[] body) => Block("switch", parameter, body);

        public static Token Case(Token guard, params Token[] body) => Block(Sentence(Literal("case"), Label(guard)), body);
        public static Token DefaultCase(params Token[] body) => Case(Literal("default"), body);

        #endregion

        #region Pointers

        public static Token Fixed(Token pointer, params Token[] body) => Block("fixed", pointer, body);
        public static Token Lock(Token reference, params Token[] body) => Block("lock", reference, body);

        #endregion

        #region Exceptions

        public static Token Catch(params Token[] body) => Block(Literal("catch"), body);
        public static Token Catch(Token exception, params Token[] body) => Block("catch", exception, body);
        public static Token Finally(params Token[] body) => Block(Literal("finally"), body);
        public static Token Try(params Token[] body) => Block(Literal("try"), body);

        #endregion

        #region Operators

        public Token this[params Token[] parameters] => Sequence(OperationType.Primary, this, Brackets(List(parameters)));

        public static Token operator +(Token expression) => Sequence(OperationType.Secondary, Literal("+"), expression);
        public static Token operator -(Token expression) => Sequence(OperationType.Secondary, Literal("-"), expression);
        public static Token operator !(Token expression) => Sequence(OperationType.Secondary, Literal("!"), expression);
        public static Token operator ~(Token expression) => Sequence(OperationType.Secondary, Literal("~"), expression);

        public static Token operator *(Token left, Token right) => Sentence(OperationType.Multiplicative, left, Literal("*"), right);
        public static Token operator /(Token left, Token right) => Sentence(OperationType.Multiplicative, left, Literal("/"), right);
        public static Token operator %(Token left, Token right) => Sentence(OperationType.Multiplicative, left, Literal("%"), right);

        public static Token operator +(Token left, Token right) => Sentence(OperationType.Additive, left, Literal("+"), right);
        public static Token operator -(Token left, Token right) => Sentence(OperationType.Additive, left, Literal("-"), right);

        public static Token operator <<(Token left, int right) => LeftShift(left, right);
        public static Token operator >> (Token left, int right) => RightShift(left, right);

        public static Token operator <(Token left, Token right) => Sentence(OperationType.Relational, left, Literal("<"), right);
        public static Token operator >(Token left, Token right) => Sentence(OperationType.Relational, left, Literal(">"), right);
        public static Token operator <=(Token left, Token right) => Sentence(OperationType.Relational, left, Literal("<="), right);
        public static Token operator >=(Token left, Token right) => Sentence(OperationType.Relational, left, Literal(">="), right);

        public static Token operator ==(Token left, Token right) => Sentence(OperationType.Equality, left, Literal("!="), right);
        public static Token operator !=(Token left, Token right) => Sentence(OperationType.Equality, left, Literal("=="), right);

        public static Token operator &(Token left, Token right) => left.IsLogical ? Sentence(OperationType.AndAlso, left, Literal("&&"), right) : Sentence(OperationType.And, left, Literal("&"), right);

        public static Token operator ^(Token left, Token right) => Sentence(OperationType.ExclusiveOr, left, Literal("^"), right);

        public static Token operator |(Token left, Token right) => left.IsLogical ? Sentence(OperationType.OrElse, left, Literal("||"), right) : Sentence(OperationType.Or, left, Literal("|"), right);

        public static Token Checked(Token expression) => Sequence(OperationType.Primary, Literal("checked"), expression);
        public static Token Default(Token type) => Sequence(OperationType.Primary, Literal("default"), Parentheses(type));
        public static Token Default<T>() => Default(Type<T>());
        public static Token Delegate(Token expression) => Sequence(OperationType.Primary, Literal("delegate"), expression);
        public static Token NameOf(Token type) => Sequence(OperationType.Primary, Literal("nameof"), Parentheses(type));
        public static Token NameOf<T>() => NameOf(Type<T>());
        public static Token SizeOf(Token type) => Sequence(OperationType.Primary, Literal("sizeof"), Parentheses(type));
        public static Token SizeOf<T>() => SizeOf(Type<T>());
        public static Token StackAlloc(Token expression) => Sequence(OperationType.Primary, Literal("stackalloc"), expression);
        public static Token TypeOf(Token type) => Sequence(OperationType.Primary, Literal("typeof"), Parentheses(type));
        public static Token TypeOf<T>() => TypeOf(Type<T>());
        public static Token Unchecked(Token expression) => Sequence(OperationType.Primary, Literal("unchecked"), expression);

        public static Token AddressOf(Token expression) => Sequence(OperationType.Secondary, Literal("&"), expression);
        public static Token Await(Token expression) => Sentence(OperationType.Secondary, Literal("await"), expression);
        public static Token Cast(Token type, Token expression) => Sequence(OperationType.Secondary, Parentheses(type), expression);
        public static Token Cast<T>(Token expression) => Cast(Type<T>(), expression);
        public static Token Decrement(Token expression) => Sequence(OperationType.Secondary, Literal("--"), expression);
        public static Token Increment(Token expression) => Sequence(OperationType.Secondary, Literal("++"), expression);
        public static Token IndexFromEnd(Token index) => Sequence(OperationType.Secondary, Literal("^"), index);
        public static Token PointerIndirection(Token expression) => Sequence(OperationType.Secondary, Literal("*"), expression);

        public static Token EndAt(Token end) => Sequence(OperationType.Range, Literal(".."), end);
        public static Token Range(Token start, Token end) => Sequence(OperationType.Range, start, Literal(".."), end);
        public static Token StartAt(Token start) => Sequence(OperationType.Range, start, Literal(".."));

        public static Token LeftShift(Token left, Token right) => Sentence(OperationType.Shift, left, Literal("<<"), right);
        public static Token RightShift(Token left, Token right) => Sentence(OperationType.Shift, left, Literal(">>"), right);

        public static Token Condition(Token test, Token isTrue, Token isFalse) => Sequence(OperationType.Conditional, test, Literal("?"), isTrue, Literal(":"), isFalse);

        public Token ConditionalIndexer(params Token[] parameters) => Sequence(OperationType.Primary, Literal("?"), Brackets(List(parameters)));
        public Token ConditionalMemberAccess(Token member) => Sequence(OperationType.Primary, this, Literal("?."), member);
        public Token ConditionalMemberAccess(string name) => ConditionalMemberAccess(Literal(name));
        public Token Decrement() => Sequence(OperationType.Primary, this, Literal("--"));
        public Token Increment() => Sequence(OperationType.Primary, this, Literal("++"));
        public Token Invoke(params Token[] parameters) => Sequence(OperationType.Primary, this, Parentheses(List(parameters)));
        public Token MemberAccess(Token member) => Sequence(OperationType.Primary, this, Literal("."), member);
        public Token MemberAccess(string name) => MemberAccess(Literal(name));
        public Token NullForgiving() => Sequence(OperationType.Primary, this, Literal("!"));
        public Token PointerMemberAccess(Token member) => Sequence(OperationType.Primary, this, Literal("->"), member);

        public Token Switch(Token expression) => Sentence(OperationType.Pattern, this, Literal("switch"), expression);
        public Token With(Token expression) => Sentence(OperationType.Pattern, this, Literal("with"), expression);

        public Token As(Token type) => Sentence(OperationType.Relational, this, Literal("as"), type);
        public Token As<T>() => As(Type<T>());
        public Token Is(Token expression) => Sentence(OperationType.Relational, this, Literal("is"), expression);
        public Token IsNot(Token expression) => Sentence(OperationType.Relational, this, Literal("is not"), expression);

        public Token And(Token expression) => Sentence(OperationType.And, this, Literal("and"), expression);

        public Token Or(Token expression) => Sentence(OperationType.Or, this, Literal("or"), expression);

        public Token Coalesce(Token expression) => Sentence(OperationType.Coalesce, this, Literal("??"), expression);

        public Token AddAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("+="), expression);
        public Token AndAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("&="), expression);
        public Token Assign(Token expression) => Sentence(OperationType.Assignment, this, Literal("="), expression);
        public Token CoalesceAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("??="), expression);
        public Token DivideAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("/="), expression);
        public Token ExclusiveOrAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("^="), expression);
        public Token Lambda(Token expression) => Sentence(OperationType.Assignment, this, Literal("=>"), expression);
        public Token LeftShiftAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("<<="), expression);
        public Token ModuloAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("%="), expression);
        public Token MultiplyAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("*="), expression);
        public Token OrAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("|="), expression);
        public Token RightShiftAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal(">>="), expression);
        public Token SubtractAssign(Token expression) => Sentence(OperationType.Assignment, this, Literal("-="), expression);

        #endregion

        //TODO add enum

        #region Members

        public sealed class Accessor
        {
            public Token Accessibility { get; set; }
            public IReadOnlyList<Token> Body { get; set; }

            internal static Token ToToken(Accessor accessor, string name)
            {
                if (accessor == null) return Empty();
                var token = ReferenceEquals(accessor.Accessibility, null) ? Literal(name) : Sentence(accessor.Accessibility, Literal(name));
                return accessor.Body == null ? Statement(token) : Block(token, accessor.Body);
            }
        }

        public sealed class GenericType
        {
            public IReadOnlyList<Token> Constraints { get; set; }
            public string Name { get; set; }
        }

        public abstract class Member
        {
            public Token Accessibility { get; set; }
            public IReadOnlyList<Token> Attributes { get; set; } = Array.Empty<Token>();
            public IReadOnlyList<Token> Modifiers { get; set; } = Array.Empty<Token>();
            public string Name { get; set; }

            protected abstract Token MemberType { get; }

            public static implicit operator Token(Member member)
            {
                return member.ToToken();
            }

            public override string ToString()
            {
                return ToToken().ToString();
            }

            internal virtual Token ToToken()
            {
                Validate();
                return Sentence(Accessibility, Sentence(Modifiers), MemberType, Literal(Name));
            }

            internal virtual void Validate(string location = "tokenization")
            {
                Validate(Name, nameof(Name), location);
                Validate(Accessibility, nameof(Accessibility), location);
                Validate(Attributes, nameof(Attributes), location);
                Validate(Modifiers, nameof(Modifiers), location);
            }

            internal Token GroupWithAttributes(Token token)
            {
                var attributes = new Token[Attributes.Count];

                for (var index = 0; index < attributes.Length; index++)
                {
                    attributes[index] = Brackets(Attributes[index]);
                }

                return Group(Group(attributes), token);
            }

            protected void Validate<T>(T value, string name, string location) where T : class
            {
                if (ReferenceEquals(value, null))
                {
                    throw new InvalidOperationException(Name != null ? $"{Name} at {location} is missing {name}" : $"{location} is missing {name}");
                }
            }
        }

        public abstract class GenericMember : Member
        {
            public IReadOnlyList<GenericType> GenericTypes { get; set; }

            protected Token GetGenericParameters()
            {
                if (GenericTypes == null) return Empty();

                var index = 0;
                var parameters = new Token[GenericTypes.Count];

                foreach (var genericType in GenericTypes)
                {
                    parameters[index++] = Literal(genericType.Name);
                }

                return Sequence(Literal("<"), List(parameters), Literal(">"));
            }

            protected Token GetGenericConstraints()
            {
                if (GenericTypes == null) return Empty();

                var index = 0;
                var constraints = new Token[GenericTypes.Count];

                foreach (var genericType in GenericTypes)
                {
                    constraints[index++] = Sentence(Sentence(Literal("where"), Literal(genericType.Name), Literal(":")), List(genericType.Constraints));
                }

                return Sentence(constraints);
            }
        }

        public abstract class MethodBase : GenericMember
        {
            public IReadOnlyList<Token> Body { get; set; } = Array.Empty<Token>();
            public IReadOnlyList<Token> Parameters { get; set; } = Array.Empty<Token>();

            internal override void Validate(string location = "tokenization")
            {
                base.Validate(location);
                Validate(Body, nameof(Body), location);
                Validate(Parameters, nameof(Parameters), location);
            }
        }

        public sealed class Field : Member
        {
            public Token FieldType { get; set; }

            protected override Token MemberType => FieldType;

            internal override Token ToToken()
            {
                return GroupWithAttributes(Statement(base.ToToken()));
            }

            internal override void Validate(string location = "tokenization")
            {
                base.Validate(location);
                Validate(FieldType, nameof(FieldType), location);
            }
        }

        public sealed class Property : Member
        {
            public Accessor Get { get; set; }
            public Accessor Set { get; set; }
            public Token PropertyType { get; set; }

            protected override Token MemberType => PropertyType;

            internal override Token ToToken()
            {
                if (Get == null && Set == null)
                {
                    Get = Set = new Accessor();
                }

                var token = base.ToToken();
                var accessors = new[] { Accessor.ToToken(Get, "get"), Accessor.ToToken(Set, "set") };

                if (Get?.Body != null || Set?.Body != null)
                {
                    token = Block(token, accessors);
                }
                else
                {
                    token = Sentence(token, Braces(Sentence(accessors)));
                }

                return GroupWithAttributes(token);
            }

            internal override void Validate(string location = "tokenization")
            {
                base.Validate(location);
                Validate(PropertyType, nameof(PropertyType), location);
            }
        }

        //TODO add base constructor
        public sealed class Constructor : MethodBase
        {
            protected override Token MemberType => Empty();

            internal override Token ToToken()
            {
                return GroupWithAttributes(Block(Sequence(base.ToToken(), Parentheses(List(Parameters))), Body));
            }
        }

        public sealed class Method : MethodBase
        {
            public Token ReturnType { get; set; }

            protected override Token MemberType => ReturnType;

            internal override Token ToToken()
            {
                return GroupWithAttributes(Block(Sentence(Sequence(base.ToToken(), GetGenericParameters(), Parentheses(List(Parameters))), GetGenericConstraints()), Body));
            }

            internal override void Validate(string location = "tokenization")
            {
                base.Validate(location);
                Validate(ReturnType, nameof(ReturnType), location);
            }
        }

        //TODO add base type
        public sealed class Class : GenericMember
        {
            public IReadOnlyList<Constructor> Constructors { get; set; } = Array.Empty<Constructor>();
            public IReadOnlyList<Field> Fields { get; set; } = Array.Empty<Field>();
            public IReadOnlyList<Method> Methods { get; set; } = Array.Empty<Method>();
            public IReadOnlyList<Property> Properties { get; set; } = Array.Empty<Property>();
            public IReadOnlyList<string> Imports { get; set; } = Array.Empty<string>();
            public string Namespace { get; set; }

            protected override Token MemberType => Literal("class");

            internal override Token ToToken()
            {
                var index = 0;
                var groups = new IEnumerable<Member>[] { Fields, Constructors, Properties, Methods };
                var members = new Token[Fields.Count + Constructors.Count + Properties.Count + Methods.Count];
                var imports = new Token[Imports.Count];

                foreach (var group in groups)
                {
                    if (group == null) continue;

                    foreach (var member in group)
                    {
                        members[index++] = member.ToToken();
                    }
                }

                index = 0;

                foreach (var import in Imports)
                {
                    imports[index++] = Statement(Sentence(Literal("using"), Literal(import)));
                }

                var @namespace = Sentence(Literal("namespace"), Literal(Namespace));
                var token = imports.Length != 0 ? Lines(Lines(imports), @namespace) : @namespace;

                return Block(token, new[] { GroupWithAttributes(Block(Sentence(Sequence(base.ToToken(), GetGenericConstraints()), GetGenericConstraints()), members)) });
            }

            internal override void Validate(string location = "tokenization")
            {
                base.Validate(location);
                Validate(Constructors, nameof(Constructors), location);
                Validate(Fields, nameof(Fields), location);
                Validate(Methods, nameof(Methods), location);
                Validate(Properties, nameof(Properties), location);
                Validate(Imports, nameof(Imports), location);
                Validate(Namespace, nameof(Namespace), location);

                for (var index = 0; index < Constructors.Count; index++)
                {
                    Constructors[index].Validate($"{Name} constructor index {index}");
                }

                for (var index = 0; index < Fields.Count; index++)
                {
                    Fields[index].Validate($"{Name} fields index {index}");
                }

                for (var index = 0; index < Methods.Count; index++)
                {
                    Methods[index].Validate($"{Name} method index {index}");
                }

                for (var index = 0; index < Properties.Count; index++)
                {
                    Properties[index].Validate($"{Name} property index {index}");
                }
            }
        }

        #endregion
    }
}