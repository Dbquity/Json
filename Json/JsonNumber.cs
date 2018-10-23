namespace Dbquity {
    using System.Globalization;

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