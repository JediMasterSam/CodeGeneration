using System;
using System.Collections.Generic;
using System.Linq;
using CodeGeneration;
using static CodeGeneration.Token;

namespace Test;

public static class Program
{
    public static void Main()
    {
        var suit = CreateFlags("Suit", "Heart", "Spade", "Club", "Diamond");

        var type = Literal("Vector3");
        var parameters = new[] { Literal("x"), Literal("y"), Literal("z") };
        var properties = new[] { Literal("X"), Literal("Y"), Literal("Z") };

        var vector = new StructTemplate
        {
            Accessibility = Public(),
            Name = type.ToString(),
            Namespace = "Demo",
            Constructors = new[]
            {
                new ConstructorTemplate
                {
                    Accessibility = Public(),
                    Parameters = parameters.Select(Parameter<float>).ToArray(),
                    Body = properties.Zip(parameters, (property, parameter) => property.Assign(parameter)).ToArray()
                }
            },
            Properties = properties.Select(property => new PropertyTemplate
            {
                Accessibility = Public(),
                PropertyType = Type<float>(),
                Name = property.ToString(),
                Get = new AccessorTemplate()
            }).ToArray(),
            Methods = new[]
            {
                BinaryVectorOperation("Add", type, (left, right) => left + right, properties),
                BinaryVectorOperation("Subtract", type, (left, right) => left - right, properties),
                BinaryVectorOperation("Multiply", type, (left, right) => left * right, properties),
                BinaryVectorOperation("Divide", type, (left, right) => left / right, properties)
            }
        };
        
        Console.WriteLine(suit);
        Console.WriteLine();
        Console.WriteLine(vector);
    }

    private static EnumTemplate CreateFlags(string name, params string[] values)
    {
        return new EnumTemplate
        {
            Accessibility = Public(),
            Attributes = new[] { Literal("Flags") },
            Name = name,
            Namespace = "Demo",
            Values = values.Select((value, index) => Literal(value).Assign(1 << index)).ToArray()
        };
    }

    private static MethodTemplate BinaryVectorOperation(string name, Token type, Func<Token, Token, Token> operation, IEnumerable<Token> values)
    {
        var other = Literal("other");

        return new MethodTemplate
        {
            Accessibility = Public(),
            Name = name,
            ReturnType = type,
            Parameters = new[] { Parameter(type, other) },
            Body = new[]
            {
                Return(New(type.Invoke(values.Select(value => operation(value, other.MemberAccess(value))).ToArray())))
            }
        };
    }
}