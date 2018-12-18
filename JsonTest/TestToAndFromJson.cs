using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Dbquity.Test {
    [TestClass]
    public class TestToAndFromJson {
        [TestMethod]
        public void TestClass() {
            TestClass tc = new TestClass();
            string classJson = "{\r\n" +
                "    \"PrivateAutoGetSetProperty\": 33,\r\n" +
                "    \"InternalAutoGetSetProperty\": 34,\r\n" +
                "    \"ProtectedAutoGetSetProperty\": 36,\r\n" +
                "    \"InternalProtectedAutoGetSetProperty\": 38,\r\n" +
                "    \"PublicAutoGetSetProperty\": 40,\r\n" +
                "    \"InternalAutoGetPrivateSetProperty\": 66,\r\n" +
                "    \"ProtectedAutoGetPrivateSetProperty\": 68,\r\n" +
                "    \"InternalProtectedAutoGetPrivateSetProperty\": 70,\r\n" +
                "    \"PublicAutoGetPrivateSetProperty\": 72,\r\n" +
                "    \"privateField\": 1,\r\n" +
                "    \"internalField\": 2,\r\n" +
                "    \"protectedField\": 4,\r\n" +
                "    \"internalProtectedField\": 6,\r\n" +
                "    \"PublicField\": 8\r\n" +
                "}";
            Assert.AreEqual(classJson, JsonValue.ToJson(tc).Format());
            tc = JsonValue.Parse(classJson).FromJson<TestClass>();
            Assert.AreEqual(classJson, JsonValue.ToJson(tc).Format());
        }
        [TestMethod]
        public void TestStruct() {
            TestStruct ts = new TestStruct();
            ts.SetAll();
            string structJson = "{\r\n" +
                "    \"PrivateAutoGetSetProperty\": 33,\r\n" +
                "    \"InternalAutoGetSetProperty\": 34,\r\n" +
                "    \"PublicAutoGetSetProperty\": 40,\r\n" +
                "    \"InternalAutoGetPrivateSetProperty\": 66,\r\n" +
                "    \"PublicAutoGetPrivateSetProperty\": 72,\r\n" +
                "    \"privateField\": 1,\r\n" +
                "    \"internalField\": 2,\r\n" +
                "    \"PublicField\": 8\r\n" +
                "}";
            Assert.AreEqual(structJson, JsonValue.ToJson(ts).Format());
            ts = JsonValue.Parse(structJson).FromJson<TestStruct>();
            Assert.AreEqual(structJson, JsonValue.ToJson(ts).Format());
        }
        [TestMethod]
        public void AnonymousTypeToJson() {
            JsonValue json =
                new {
                    link = "http://www.bbc.co.uk",
                    show = "Teletubbies",
                    stats = new { year = 2017, viewers = 23456789 }
                }.ToJson();
            Assert.AreEqual<string>("http://www.bbc.co.uk", json["link"]);
            Assert.AreEqual<string>("Teletubbies", json["show"]);
            Assert.AreEqual(@"{""year"":2017,""viewers"":23456789}", json["stats"].ToString());
            Assert.AreEqual<int>(2017, json["stats"]["year"]);
            var point = new { x = 3, y = 89 };
            Assert.AreEqual(@"{""x"":3,""y"":89}", point.ToJsonString());
            string formattedJson =
                @"{\" +
                @"    ""x"": 3,\" +
                @"    ""y"": 89\" +
                @"}";
            Assert.AreEqual(formattedJson.Replace(@"\", Environment.NewLine), point.ToFormattedJson());
        }
        class OnlyIndexers {
            public int this[int i] => i;
            public string this[string s] => s;
        }
        class WithIndexers : OnlyIndexers {
            public string Name = nameof(WithIndexers);
        }
        [TestMethod]
        public void Indexer() {
            Assert.AreEqual("Cannot create Json from the type, 'System.Object'",
                Assert.ThrowsException<NotSupportedException>(() => new object().ToJson()).Message);
            Assert.AreEqual("Cannot create Json from the type, 'Dbquity.Test.TestToAndFromJson+OnlyIndexers'",
                Assert.ThrowsException<NotSupportedException>(() => new OnlyIndexers().ToJson()).Message);
            Assert.AreEqual(@"{""Name"":""WithIndexers""}", new WithIndexers().ToJsonString());
        }
        [TestMethod]
        public void JsonToJson() {
            JsonValue json = new WithIndexers().ToJson();
            Assert.AreSame(json, json.ToJson());
        }
        [TestMethod]
        public void UnicodeText() {
            // see https://www.dropbox.com/developers/reference/json-encoding
            string text = "\"some_üñîcødé_and_\x7f\"";
            JsonText jsonText = JsonText.Parse(text);
            Assert.AreEqual(@"some_\u00fc\u00f1\u00eec\u00f8d\u00e9_and_\u007f", jsonText.Value);
            Assert.AreEqual(text.Trim('\"'), jsonText.FromJson<string>());
            text = "\"Welcome 🤗 to a link🔗\"";
            jsonText = JsonText.Parse(text);
            Assert.AreEqual(@"Welcome \ud83e\udd17 to a link\ud83d\udd17", jsonText.Value);
            Assert.AreEqual(text.Trim('\"'), jsonText.FromJson<string>());
            text = "\"More than\rone line.\nThree in fact!\r\nNo four :-)\"";
            jsonText = JsonText.Parse(text);
            Assert.AreEqual(@"More than\u000done line.\u000aThree in fact!\u000d\u000aNo four :-)", jsonText.Value);
            Assert.AreEqual(text.Trim('\"'), jsonText.FromJson<string>());
        }
    }
}