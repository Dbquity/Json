namespace Dbquity {
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class JsonArray : JsonValue, IList<JsonValue> {
        public static new JsonArray Parse(string text) => JsonValue.Parse(text).Array;
        public readonly List<JsonValue> Items = new List<JsonValue>();
        public JsonArray() { }
        public JsonArray(params JsonValue[] items) { Items.AddRange(items); }
        public override string ToString() => $"[{string.Join(",", Items.Select(v => v.ToString()))}]";
        public override string Format(int indent = 0) => $"[{NewLine(indent + 1)}" +
            $"{string.Join($",{NewLine(indent + 1)}", Items.Select(v => v.Format(indent + 1)))}{NewLine(indent)}]";
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
}