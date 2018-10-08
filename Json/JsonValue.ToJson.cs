using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dbquity {
    public static class ObjectJsonExtensions {
        public static JsonValue ToJson(this object value) => JsonValue.ToJson(value);
    }
    partial class JsonValue {
        public static JsonValue ToJson(object value) =>
            ToJson(value, new Dictionary<object, JsonValue>());
        static JsonValue ToJson(object value, Dictionary<object, JsonValue> visited) {
            switch (value) {
                case null: return JsonNull.Instance;
                case bool b: return new JsonBool(b);
                case byte b: return new JsonNumber(b);
                case int i: return new JsonNumber(i);
                case uint u: return new JsonNumber(u);
                case long l: return new JsonNumber(l);
                case ulong u: return new JsonNumber(u);
                case decimal d: return new JsonNumber(d);
                case float f: return new JsonNumber(f);
                case double d: return new JsonNumber(d);
                case Guid g:
                    if (g == default)
                        return JsonNull.Instance;
                    else
                        return new JsonText(g.ToString());
                case DateTime d: return new JsonText(d.ToString(CultureInfo.InvariantCulture));
                case TimeSpan t: return new JsonText(t.ToString());
                case Version v: return new JsonText(v.ToString());
                case Type tp: return new JsonText(tp.AssemblyQualifiedName);
                case char c: return new JsonText(c);
                case string s: return new JsonText(s);
                case IEnumerable ie:
                    return Visit(() => new JsonArray(Enumerate(ie).ToArray()));
                    IEnumerable<JsonValue> Enumerate(IEnumerable enumerable) {
                        foreach (object o in enumerable)
                            yield return ToJson(o);
                    }
            }
            Type type = value.GetType();
            TypeInfo info = type.GetTypeInfo();
            if (info.IsClass || info.IsValueType) {
                IEnumerable<MemberInfo> mis = GetPropertiesAndFields(info);
                Func<MemberInfo, string> getName = type.IsSealed && type.Name.Contains(">f__AnonymousType") ?
                    mi => mi.Name.Substring(1, mi.Name.Length - "<>i__Field".Length) :
                    (Func<MemberInfo, string>)(mi => mi.Name);
                if (mis.Any())
                    return Visit(() => new JsonObject(mis.Select(mi => (getName(mi), ToJson(GetValue(mi), visited))).
                        Where(kv => kv.Item2 != JsonNull.Instance)));
                object GetValue(MemberInfo mi) {
                    if (mi is PropertyInfo pi)
                        return pi.GetValue(value);
                    if (mi is FieldInfo fi)
                        return fi.GetValue(value);
                    throw new InvalidProgramException();
                }
            }
            throw new NotSupportedException($"Cannot create Json from the type, '{type.FullName}'");

            JsonValue Visit(Func<JsonValue> produce) {
                if (!visited.TryGetValue(value, out JsonValue json)) {
                    json = produce();
                    visited.Add(value, json);
                }
                return json;
            }
        }
        internal static IEnumerable<MemberInfo> GetPropertiesAndFields(TypeInfo ti) {
            if (ti.AsType() != typeof(object)) {
                Type b = ti.BaseType;
                foreach (MemberInfo mi in GetPropertiesAndFields(b.GetTypeInfo()))
                    yield return mi;
                foreach (MemberInfo mi in ti.DeclaredMembers)
                    if (mi is PropertyInfo pi && !(pi.GetMethod?.IsStatic ?? true) && pi.CanRead && pi.CanWrite)
                        yield return mi;
                    else if (mi is FieldInfo fi && !fi.IsStatic && !fi.Name.EndsWith(">k__BackingField"))
                        yield return mi;
            }
        }
    }
}
