using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CodeGeneration;
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

            var temp = new EnumTemplate
            {
                Accessibility = Public(),
                Name = "Test",
                Namespace = "Test",
                Attributes = new []{Literal("Flags")},
                Imports = new []{"System"},
                BaseTypes = new []{Type<long>()},
                Values = new[]
                {
                    Literal("Default").Assign(0),
                    Literal("On").Assign(1),
                    Literal("Off").Assign(2)
                }
            };
            
            Console.WriteLine(temp.ToString());
            
            var foobar = new ClassTemplate
            {
                Attributes = new[] { Literal("Foobar").Invoke(Literal("BindingFlags").Assign(BindingFlags.Public | BindingFlags.Static)) },
                BaseTypes = new[] { Type<IEnumerable>(), Type<IComparable>() },
                GenericTypes = new[]
                {
                    new GenericTypeTemplate
                    {
                        Name = "TEnumerable",
                        Constraints = new[] { Type<IEnumerable>() }
                    }
                },
                Imports = new[] { "System", "System.Collections" },
                Accessibility = Public(),
                Name = "Foobar",
                Namespace = "Test",
                Fields = new[]
                {
                    new FieldTemplate
                    {
                        Attributes = new[] { Literal("Foobar") },
                        Accessibility = Private(),
                        Name = "_name",
                        FieldType = Type<string>()
                    }
                },
                Properties = new[]
                {
                    new PropertyTemplate
                    {
                        Attributes = new[] { Literal("Foobar") },
                        Accessibility = Public(),
                        Name = "Name",
                        PropertyType = Type<string>(),
                        Get = new AccessorTemplate
                        {
                            Body = new[]
                            {
                                Return(Literal("_name"))
                            }
                        }
                    },
                    new PropertyTemplate
                    {
                        Accessibility = Internal(),
                        Name = "Count",
                        PropertyType = Type<int>()
                    }
                },
                Constructors = new[]
                {
                    new ConstructorTemplate
                    {
                        Accessibility = Public(),
                        Parameters = new []{Parameter<string>("name")},
                        BaseParameters = new[]{Literal("name")}
                    }
                },
                Methods = new[]
                {
                    new MethodTemplate
                    {
                        Attributes = new[] { Literal("Foobar") },
                        Accessibility = Public(),
                        Name = "Foobar",
                        Modifiers = new[] { Static() },
                        GenericTypes = new[] { new GenericTypeTemplate { Name = "T", Constraints = new[] { Type<IList>() } } },
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