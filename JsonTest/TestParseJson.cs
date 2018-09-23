using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Dbquity.Test {
    [TestClass]
    public class TestParseJson {
        [TestMethod]
        public void ParseLiteral() {
            Assert.IsFalse(JsonBool.Parse("false").Value);
            Assert.IsTrue(JsonBool.Parse("true").Value);
            Assert.AreEqual("false", new JsonBool(false).ToString());
            Assert.AreEqual("true", new JsonBool(true).ToString());
            Assert.AreEqual("id", JsonText.Parse("\"id\"").Value);
            Assert.AreEqual(89, JsonNumber.Parse("89").Byte);
            Assert.AreEqual("\"{<guid>}\"", JsonText.Parse("\"\\\"{<guid>}\\\"\"").Value);
            Assert.AreEqual(-0.5m, JsonNumber.Parse("-0.5").Decimal);
            Assert.AreEqual(500d, JsonNumber.Parse("+0.5E3").Double);
            Assert.AreEqual(0.0005d, JsonNumber.Parse("0.5E-3").Double);
            JsonObject o = new JsonObject(("name", new JsonText("Lars \"kodekarl\" Hammer")));
            const string json = "{\"name\":\"Lars \\\"kodekarl\\\" Hammer\"}";
            Assert.AreEqual(json, o.ToString());
            Assert.AreEqual(json, JsonValue.Parse(json).ToString());
            Assert.AreEqual(json, JsonValue.Parse(o.Format()).ToString());
        }
        [TestMethod]
        public void ParseObject() {
            JsonValue value = JsonValue.Parse("{\"displayName\":\"Lars Hammer\", \"id\":\"89\"}");
            Assert.AreEqual(typeof(JsonObject), value.GetType());
            JsonObject o = value.Object;
            Assert.AreEqual(2, o.Properties.Count);
            Assert.AreEqual("displayName", o.Properties.Keys.First());
            Assert.AreEqual(typeof(JsonText), o["displayName"].GetType());
            Assert.AreEqual("Lars Hammer", o["displayName"].Text.Value);
            Assert.AreEqual("id", o.Properties.Keys.Last());
            Assert.AreEqual(typeof(JsonText), o["id"].GetType());
            Assert.AreEqual("89", o["id"].Text.Value);
            Assert.AreEqual("{\r\n    \"displayName\": \"Lars Hammer\",\r\n    \"id\": \"89\"\r\n}", value.Format());
            // {
            //     "displayName": "Lars Hammer",
            //     "id": "89"
            // }
            Assert.AreEqual<string>("Lars Hammer", value["displayName"]);
            Assert.AreEqual<string>("89", value["id"]);
        }
        [TestMethod]
        public void ParseArray() {
            JsonValue value = JsonValue.Parse("[1,2,3,4,5]");
            Assert.AreEqual(typeof(JsonArray), value.GetType());
            Assert.AreEqual(5, value.Array.Items.Count);
            Assert.AreEqual(typeof(JsonNumber), value.Array[2].GetType());
            Assert.AreEqual(new JsonNumber { Int = 3 }, value.Array[2].Number);
            JsonObject o = new JsonObject();
            o["name"] = new JsonText { Value = "Lars" };
            o["age"] = new JsonNumber { Int = 50 };
            JsonArray a = new JsonArray();
            a.Items.Add(o);
            JsonObject p = new JsonObject();
            p["name"] = "Bodil";
            Assert.AreEqual("Bodil", p["name"].Text.Value);
            p["age"] = 50;
            Assert.AreEqual<int>(50, p["age"]);
            p["husbond"] = o;
            a.Items.Add(p);
            string arrayText = a.ToString();
            Assert.AreEqual(JsonValue.Parse(arrayText).Format(), a.Format());
            Assert.AreEqual(100, 2 * a[1]["age"]);
        }
        [TestMethod]
        public void ParseOneDriveJsonExample() {
            JsonObject childrenOfRoot = JsonObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "OneDriveJsonExample.txt")));
            JsonArray children = childrenOfRoot["value"].Array;
            Assert.AreEqual<string>("Contoso Finance Dashboard", childrenOfRoot["value"][2]["name"]);
        }
    }
}