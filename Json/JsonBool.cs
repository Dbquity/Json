namespace Dbquity {
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
}