namespace Dbquity {
    public static class ObjectJsonExtensions {
        public static string ToFormattedJson(this object value) => JsonValue.ToFormattedJson(value);
        public static string ToJsonString(this object value) => JsonValue.ToJsonString(value);
        public static JsonValue ToJson(this object value) => JsonValue.ToJson(value);
    }
}