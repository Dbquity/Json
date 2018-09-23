using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dbquity {
    partial class JsonValue {
        public T FromJson<T>() => (T)FromJson(typeof(T));
        public object FromJson(Type type) => FromJson(type, new HashSet<(MemberInfo, object)>());
        internal object FromJson(Type type, HashSet<(MemberInfo, object)> visited) {
            switch ((object)this) {
                case JsonNull n:
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
                    if (type == typeof(Version)) return Version.Parse(t.Value);
                    if (type == typeof(Type)) return Type.GetType(t.Value);
                    if (type == typeof(string)) return t.Value;
                    if (type.GetTypeInfo().IsEnum) return Enum.Parse(type, t.Value);
                    break;
            }
            throw new NotSupportedException();
        }
    }
    partial class JsonObject {
        public T FromJson<T>(params object[] ctorArgs) => (T)FromJson(typeof(T), new HashSet<(MemberInfo, object)>(), ctorArgs);
        internal object FromJson(Type type, HashSet<(MemberInfo, object)> visited, params object[] ctorArgs) {
            object obj = Activator.CreateInstance(type, ctorArgs);
            if (Properties.Any()) {
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                foreach (MemberInfo mi in GetPropertiesAndFields(obj.GetType().GetTypeInfo()))
                    members.Add(mi.Name, mi);
                foreach (var kv in Properties)
                    SetValue(kv.Key, kv.Value);

                void SetValue(string name, JsonValue json) {
                    MemberInfo mi = members[name];
                    if (json is JsonArray || json is JsonObject) {
                        if (visited.Contains((mi, obj)))
                            return;
                        visited.Add((mi, obj));
                    }
                    if (mi is PropertyInfo pi2)
                        pi2.SetValue(obj, json.FromJson(pi2.PropertyType, visited));
                    else if (mi is FieldInfo fi2)
                        fi2.SetValue(obj, json.FromJson(fi2.FieldType, visited));
                    else
                        throw new InvalidProgramException();
                }
            }
            return obj;
        }
    }
}
