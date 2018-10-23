namespace Dbquity {
    public sealed class JsonNull : JsonValue {
        private JsonNull() { }
        public static readonly JsonNull Instance = new JsonNull();
        public override string ToString() => "null";
        public override bool Equals(object obj) => obj is JsonNull;
        public override int GetHashCode() => 0;
    }
}