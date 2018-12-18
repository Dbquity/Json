using System;
using System.Text;

namespace Dbquity {
    public sealed class JsonText : JsonValue {
        public static new JsonText Parse(string text) => JsonValue.Parse(text).Text;
        private string value;
        public string Value { get => value; set => this.value = value?.Replace("\\\"", "\""); }
        public JsonText() { }
        public JsonText(string value) { Value = value; }
        public JsonText(char c) { Char = c; }
        public char Char { get => Value[0]; set => Value = value.ToString(); }
        public override string ToString() => $"\"{Value.Replace("\"", "\\\"")}\"";
        public override bool Equals(object obj) => obj is JsonText t && Value.Equals(t.Value);
        public override int GetHashCode() => Value.GetHashCode();
        public static implicit operator string(JsonText t) => t?.Value;
        public static implicit operator JsonText(string t) => new JsonText { Value = t };
        public static string EncodeUnicode(string s) {
            StringBuilder builder = new StringBuilder();
            bool foundNonAscii = false;
            foreach (char c in s)
                if ((uint)c > 0x7e || (uint)c < 0x20) {
                    builder.AppendFormat("\\u{0:x4}", (uint)c);
                    foundNonAscii = true;
                } else
                    builder.Append(c);
            return foundNonAscii ? builder.ToString() : s;
        }
        public static string DecodeUnicode(string s) {
            StringBuilder builder = new StringBuilder(s.Length);
            bool unicodeFound = false;
            int match = 0;
            uint unicode = 0;
            foreach (char c in s)
                if (match == 0 && c == '\\')
                    match = 1;
                else if (match == 1)
                    if (c == 'u')
                        match = 2;
                    else {
                        match = 0;
                        builder.Append('\\');
                        if (c != '\\')
                            builder.Append(c);
                    } else if (match > 1) {
                    byte b =
                        c >= '0' && c <= '9' ? (byte)(c - '0') :
                        c >= 'a' && c <= 'f' ? (byte)(c - 'a' + 10) :
                        c >= 'A' && c <= 'F' ? (byte)(c - 'A' + 10) :
                        throw new FormatException(formatError());
                    unicode = unicode * 16 + b;
                    if (++match == 6) {
                        match = 0;
                        builder.Append((char)unicode);
                        unicode = 0;
                        unicodeFound = true;
                    }
                } else
                    builder.Append(c);
            if (match > 1)
                throw new FormatException(formatError());
            string formatError() {
                string where = builder.ToString();
                if (where.Length > 35)
                    where = "..." + where.Substring(where.Length - 35);
                return $"\\u not followed by 4 hex digits after '{where}' in '{s}'.";
            }
            return unicodeFound ? builder.ToString() : s;
        }
    }
}