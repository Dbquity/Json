using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Dbquity {
    partial class JsonValue {
        internal object FromJson(Type type, HashSet<(MemberInfo, object)> visited) {
            switch (this) {
                case JsonNull n:
                    if (type == typeof(Guid))
                        return default(Guid);
                    return null;
                case JsonObject o:
                    return o.FromJson(type, visited);
                case JsonArray a:
                    IList list = (IList)Activator.CreateInstance(type);
                    Type elementType = type.GetTypeInfo().IsGenericType ? type.GenericTypeArguments[0] : type.GetElementType();
                    if (elementType == null)
                        throw new ArgumentException($"Could not determine element type of '{type.FullName}'");
                    foreach (JsonValue element in a.Items)
                        list.Add(element.FromJson(elementType));
                    return list;
                case JsonBool b:
                    return b.Value;
                case JsonNumber n:
                    if (type == typeof(byte)) return n.Byte;
                    if (type == typeof(sbyte)) return n.SByte;
                    if (type == typeof(int)) return n.Int;
                    if (type == typeof(uint)) return n.UInt;
                    if (type == typeof(long)) return n.Long;
                    if (type == typeof(ulong)) return n.ULong;
                    if (type == typeof(float)) return n.Float;
                    if (type == typeof(double)) return n.Double;
                    if (type == typeof(decimal)) return n.Decimal;
                    break;
                case JsonText t:
                    if (type == typeof(char)) return char.Parse(t.Value);
                    if (type == typeof(TimeSpan)) return TimeSpan.Parse(t.Value, CultureInfo.InvariantCulture);
                    if (type == typeof(DateTime)) return DateTime.Parse(t.Value, CultureInfo.InvariantCulture);
                    if (type == typeof(Guid)) return Guid.Parse(t.Value);
                    if (type == typeof(Version)) return Version.Parse(t.Value);
                    if (type == typeof(Type)) return Type.GetType(t.Value);
                    if (type == typeof(string)) return t.Value;
                    if (type.GetTypeInfo().IsEnum) return Enum.Parse(type, t.Value);
                    break;
            }
            throw new NotSupportedException();
        }
    }
}