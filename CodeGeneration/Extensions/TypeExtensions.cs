using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration
{
    public static class TypeExtensions
    {
        private static readonly IReadOnlyDictionary<Type, string> Aliases = new Dictionary<Type, string>
        {
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(bool), "bool" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(object), "object" }
        };

        public static string ToPrintable(this Type type)
        {
            if (Aliases.TryGetValue(type, out var alias)) return alias;
            if (type.IsArray) return $"{type.GetElementType().ToPrintable()}[]";

            return type.IsGenericType ? $"{type.Name.Substring(0, type.Name.IndexOf('`'))}<{string.Join(", ", type.GenericTypeArguments.Select(generic => generic.ToPrintable()))}>" : type.Name;
        }
    }
}