using System;
using System.Collections;
using System.Collections.Generic;
using static CodeGeneration.Token;

namespace Test
{
    public static class Program
    {
        public static void Main()
        {
            var a = Literal("a");
            var b = Literal("b");
            var c = Literal("c");
            var input = Literal("input");

            var foobar = new Class
            {
                Imports = new []{"System", "System.Collections"},
                Accessibility = Public(),
                Name = "Foobar",
                Namespace = "Test",
                Fields = new[]
                {
                    new Field
                    {
                        Accessibility = Private(),
                        Name = "_name",
                        FieldType = Type<string>()
                    }
                },
                Properties = new[]
                {
                    new Property
                    {
                        Accessibility = Public(),
                        Name = "Name",
                        PropertyType = Type<string>(),
                        Get = new Accessor
                        {
                            Body = new[]
                            {
                                Return(Literal("_name"))
                            }
                        }
                    },
                    new Property
                    {
                        Accessibility = Internal(),
                        Name = "Count",
                        PropertyType = Type<int>()
                    }
                },
                Methods = new[]
                {
                    new Method
                    {
                        Accessibility = Public(),
                        Name = "Foobar",
                        Modifiers = new[] { Static() },
                        GenericTypes = new[] { new GenericType { Name = "T", Constraints = new[] { Type<IList>() } } },
                        ReturnType = Void(),
                        Parameters = new[] { Parameter<int>(input) },
                        Body = new[]
                        {
                            Variable(a).Assign(12),
                            Variable(b).Assign(129 + input),
                            Variable(c).Assign(304),
                            Variable<List<int>>("list"),
                            Switch(c,
                                Case(0,
                                    a.Increment(),
                                    Break()
                                ),
                                Case(1,
                                    b.Decrement(),
                                    Break()
                                ),
                                DefaultCase(
                                    Decrement(a),
                                    Increment(b))
                            ),
                            b.MemberAccess("Properties")["params", 123m].ConditionalMemberAccess("People").MemberAccess("Sort").Invoke(a, c),
                            If(a < b,
                                a.AddAssign(b)
                            ),
                            ElseIf(a + a < b,
                                Literal("//This is a comment"),
                                a.AddAssign(b * 2),
                                b.SubtractAssign(a)
                            ),
                            Else(
                                DoWhile(a > b,
                                    b.MultiplyAssign((a + a) * b)
                                ),
                                If(b > 100,
                                    b.Assign(100)
                                )
                            )
                        }
                    }
                }
            };

            Console.Write(foobar.ToString());
        }
    }
}