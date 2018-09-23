using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}