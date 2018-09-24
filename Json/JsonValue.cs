namespace Dbquity {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    public abstract partial class JsonValue {
        internal JsonValue() { }
        public static JsonValue Parse(string text) => Parse(text, 0).value;
        public JsonObject Object => (JsonObject)this;
        public JsonArray Array => (JsonArray)this;
        public JsonText Text => (JsonText)this;
        public JsonNumber Number => (JsonNumber)this;
        public JsonBool Bool  => (JsonBool)this;
        public JsonNull Null => (JsonNull)this;
        public virtual string Format(int indent = 0) => ToString();
        internal static string NewLine(int indent) => Environment.NewLine + string.Empty.PadRight(indent * 4);
    }
    public sealed partial class JsonObject : JsonValue {
        public static new JsonObject Parse(string text) => JsonValue.Parse(text).Object;
        public Dictionary<string, JsonValue> Properties = new Dictionary<string, JsonValue>();
        public JsonObject() { }
        public JsonObject(params (string, JsonValue)[] properties) { Add(properties); }
        public JsonObject(IEnumerable<(string, JsonValue)> properties) { Add(properties); }
        public override string ToString() => $"{{{string.Join(",", Properties.Select(p => $"\"{p.Key}\":{p.Value}"))}}}";
        public override string Format(int indent = 0)
            => $"{{{NewLine(indent + 1)}{string.Join($",{NewLine(indent + 1)}", Properties.Select(p => Format(p, indent + 1)))}{NewLine(indent)}}}";
        private static string Format(KeyValuePair<string, JsonValue> property, int indent)
            => $"\"{property.Key}\": {property.Value.Format(indent)}";
        public bool TryGetPropertyValue<T>(string propertyName, out T value) where T : JsonValue {
            bool found = Properties.TryGetValue(propertyName, out JsonValue v);
            value = found ? (T)v : null;
            return found;
        }
        public new JsonValue this[string propertyName] {
            get => Properties.TryGetValue(propertyName, out JsonValue v) ? v : null;
            set { try { Properties[propertyName] = value; } catch (ArgumentOutOfRangeException) { Properties.Add(propertyName, value); } }
        }
        public bool Has(string propertyName) => Properties.ContainsKey(propertyName);
        public void Add(params (string name, JsonValue value)[] properties) => Add((IEnumerable<(string, JsonValue)>)properties);
        public void Add(IEnumerable<(string name, JsonValue value)> properties) {
            foreach (var (name, value) in properties)
                Properties.Add(name, value);
        }
    }
    public sealed class JsonArray : JsonValue, IList<JsonValue> {
        public static new JsonArray Parse(string text) => JsonValue.Parse(text).Array;
        public readonly List<JsonValue> Items = new List<JsonValue>();
        public JsonArray() { }
        public JsonArray(params JsonValue[] items) { Items.AddRange(items); }
        public override string ToString() => $"[{string.Join(",", Items.Select(v => v.ToString()))}]";
        public override string Format(int indent = 0)
            => $"[{NewLine(indent + 1)}{string.Join($",{NewLine(indent + 1)}", Items.Select(v => v.Format(indent + 1)))}{NewLine(indent)}]";
        public new JsonValue this[int i] { get => Items[i]; set => Items[i] = value; }
        public int IndexOf(JsonValue item) => Items.IndexOf(item);
        public void Insert(int index, JsonValue item) => Items.Insert(index, item);
        public void RemoveAt(int index) => Items.RemoveAt(index);
        public void Add(JsonValue item) => Items.Add(item);
        public void Clear() => Items.Clear();
        public bool Contains(JsonValue item) => Items.Contains(item);
        public void CopyTo(JsonValue[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);
        public bool Remove(JsonValue item) => Items.Remove(item);
        public int Count => Items.Count;
        public bool IsReadOnly => ((IList<JsonValue>)Items).IsReadOnly;
        public IEnumerator<JsonValue> GetEnumerator() => ((IList<JsonValue>)Items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IList<JsonValue>)Items).GetEnumerator();
    }
    public sealed class JsonText : JsonValue {
        public static new JsonText Parse(string text) => JsonValue.Parse(text).Text;
        private string value;
        public string Value { get => value; set => this.value = value.Replace("\\\"", "\""); }
        public JsonText() { }
        public JsonText(string value) { Value = value; }
        public JsonText(char c) { Char = c; }
        public char Char { get => Value[0]; set => Value = value.ToString(); }
        public override string ToString() => $"\"{Value.Replace("\"", "\\\"")}\"";
        public override bool Equals(object obj) => obj is JsonText t && Value.Equals(t.Value);
        public override int GetHashCode() => Value.GetHashCode();
        public static implicit operator string(JsonText t) => t?.Value;
        public static implicit operator JsonText(string t) => new JsonText { Value = t };
    }
    public sealed class JsonBool : JsonValue {
        public static new JsonBool Parse(string text) => JsonValue.Parse(text).Bool;
        public bool Value;
        public JsonBool() { }
        public JsonBool(bool value) { Value = value; }
        public override string ToString() => Value ? "true" : "false";
        public override bool Equals(object obj) => obj is JsonBool b && Value.Equals(b.Value);
        public override int GetHashCode() => Value.GetHashCode();
        public static implicit operator bool(JsonBool b) => b?.Value ?? default;
        public static implicit operator JsonBool(bool b) => new JsonBool { Value = b };
    }
    public sealed class JsonNull : JsonValue {
        private JsonNull() { }
        public static readonly JsonNull Instance = new JsonNull();
        public override string ToString() => "null";
        public override bool Equals(object obj) => obj is JsonNull;
        public override int GetHashCode() => 0;
    }
    public sealed class JsonNumber : JsonValue {
        public static new JsonNumber Parse(string text) => JsonValue.Parse(text).Number;
        public string String;
        public JsonNumber() { }
        public JsonNumber(byte value) { Byte = value; }
        public JsonNumber(sbyte value) { SByte = value; }
        public JsonNumber(int value) { Int = value; }
        public JsonNumber(uint value) { UInt = value; }
        public JsonNumber(long value) { Long = value; }
        public JsonNumber(ulong value) { ULong = value; }
        public JsonNumber(decimal value) { Decimal = value; }
        public JsonNumber(float value) { Float = value; }
        public JsonNumber(double value) { Double = value; }
        public byte Byte { get => byte.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public sbyte SByte { get => sbyte.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public int Int { get => int.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public uint UInt { get => uint.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public long Long { get => long.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public ulong ULong { get => ulong.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public decimal Decimal { get => decimal.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public float Float { get => float.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public double Double { get => double.Parse(String, CultureInfo.InvariantCulture); set => String = value.ToString(CultureInfo.InvariantCulture); }
        public override string ToString() => String;
        public override bool Equals(object obj) => obj is JsonNumber n && String.Equals(n.String);
        public override int GetHashCode() => String.GetHashCode();
        public static implicit operator byte(JsonNumber b) => b?.Byte ?? default;
        public static implicit operator sbyte(JsonNumber sb) => sb?.SByte ?? default;
        public static implicit operator int(JsonNumber i) => i?.Int ?? default;
        public static implicit operator uint(JsonNumber ui) => ui?.Byte ?? default;
        public static implicit operator long(JsonNumber l) => l?.Long ?? default;
        public static implicit operator ulong(JsonNumber ul) => ul?.ULong ?? default;
        public static implicit operator decimal(JsonNumber d) => d?.Decimal ?? default;
        public static implicit operator float(JsonNumber bf) => bf?.Float ?? default;
        public static implicit operator double(JsonNumber d) => d?.Double ?? default;
        public static implicit operator JsonNumber(byte b) => new JsonNumber(b);
        public static implicit operator JsonNumber(sbyte sb) => new JsonNumber(sb);
        public static implicit operator JsonNumber(int i) => new JsonNumber(i);
        public static implicit operator JsonNumber(uint ui) => new JsonNumber(ui);
        public static implicit operator JsonNumber(long l) => new JsonNumber(l);
        public static implicit operator JsonNumber(ulong ul) => new JsonNumber(ul);
        public static implicit operator JsonNumber(decimal d) => new JsonNumber(d);
        public static implicit operator JsonNumber(float f) => new JsonNumber(f);
        public static implicit operator JsonNumber(double d) => new JsonNumber(d);
    }
}
