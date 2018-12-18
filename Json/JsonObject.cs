namespace Dbquity {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public sealed class JsonObject : JsonValue {
        public static new JsonObject Parse(string text) => JsonValue.Parse(text).Object;
        public Dictionary<string, JsonValue> Properties = new Dictionary<string, JsonValue>();
        public JsonObject() { }
        public JsonObject(params (string, JsonValue)[] properties) { Add(properties); }
        public JsonObject(IEnumerable<(string, JsonValue)> properties) { Add(properties); }
        public override string ToString() => $"{{{string.Join(",", Properties.Select(p => $"\"{p.Key}\":{p.Value}"))}}}";
        public override string Format(int indent = 0) => $"{{{NewLine(indent + 1)}" +
            $"{string.Join($",{NewLine(indent + 1)}", Properties.Select(p => Format(p, indent + 1)))}{NewLine(indent)}}}";
        private static string Format(KeyValuePair<string, JsonValue> property, int indent)
            => $"\"{property.Key}\": {property.Value.Format(indent)}";
        public bool TryGetPropertyValue<T>(string propertyName, out T value) where T : JsonValue {
            bool found = Properties.TryGetValue(propertyName, out JsonValue v);
            value = found ? (T)v : null;
            return found;
        }
        public new JsonValue this[string propertyName] {
            get => Properties.TryGetValue(propertyName, out JsonValue v) ? v : null;
            set => Properties[propertyName] = value;
        }
        public bool Has(string propertyName) => Properties.ContainsKey(propertyName);
        public void Add(params (string name, JsonValue value)[] properties) => Add((IEnumerable<(string, JsonValue)>)properties);
        public void Add(IEnumerable<(string name, JsonValue value)> properties) {
            foreach (var (name, value) in properties)
                Properties.Add(name, value);
        }
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