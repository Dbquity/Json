namespace Dbquity {
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
}