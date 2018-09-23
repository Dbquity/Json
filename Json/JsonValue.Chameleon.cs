namespace Dbquity {
    partial class JsonValue {
        public JsonValue this[string propertyName] { get => Object[propertyName]; set => Object[propertyName] = value; }
        public JsonValue this[int index] { get => Array[index]; set => Array[index] = value; }
        public static implicit operator JsonValue(string value) => new JsonText(value);
        public static implicit operator JsonValue(bool value) => new JsonBool(value);
        public static implicit operator JsonValue(byte b) => new JsonNumber(b);
        public static implicit operator JsonValue(sbyte sb) => new JsonNumber(sb);
        public static implicit operator JsonValue(int i) => new JsonNumber(i);
        public static implicit operator JsonValue(uint ui) => new JsonNumber(ui);
        public static implicit operator JsonValue(long l) => new JsonNumber(l);
        public static implicit operator JsonValue(ulong ul) => new JsonNumber(ul);
        public static implicit operator JsonValue(decimal d) => new JsonNumber(d);
        public static implicit operator JsonValue(float f) => new JsonNumber(f);
        public static implicit operator JsonValue(double d) => new JsonNumber(d);        
        public static implicit operator string(JsonValue value) => value.Text.Value;
        public static implicit operator bool(JsonValue value) => value.Bool.Value;
        public static implicit operator byte(JsonValue value) => value.Number.Byte;
        public static implicit operator sbyte(JsonValue value) => value.Number.SByte;
        public static implicit operator int(JsonValue value) => value.Number.Int;
        public static implicit operator uint(JsonValue value) => value.Number.UInt;
        public static implicit operator long(JsonValue value) => value.Number.Long;
        public static implicit operator ulong(JsonValue value) => value.Number.ULong;
        public static implicit operator decimal(JsonValue value) => value.Number.Decimal;
        public static implicit operator float(JsonValue value) => value.Number.Float;
        public static implicit operator double(JsonValue value) => value.Number.Double;
    }
}
