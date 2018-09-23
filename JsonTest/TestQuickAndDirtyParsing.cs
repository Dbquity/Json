using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dbquity.Test {
    [TestClass]
    public class TestQuickAndDirtyParsing {
    static string QuickAndDirtyJsonLookupText(string json, string key) =>
        QuickAndDirtyJsonFind(json, key).Split('"')[1];
    static string QuickAndDirtyJsonFind(string json, string key) {
        int keyIndex = json.IndexOf($"\"{key}\":");
        if (keyIndex < 0)
            throw new KeyNotFoundException($"Key \"{key}\" not found in '{json}'.");
        return json.Substring(keyIndex + 3 + key.Length).TrimStart();
    }
        [TestMethod]
        public void SampleFromAliceOnStackOverflow() {
            const string json =
                "{"+
                "  \"User\":{" +
                "     \"username\":\"Vinayaka\"," +
                "     \"email\":\"Vinayaka@mindsol.in\"," +
                "     \"ref_id\":\"43523543\"," +
                "     \"state_code\":\"UP\"," +
                "     \"active_status\":\"1\"," +
                "     \"user_type\":\"Admin\"," +
                "     \"last_active\":\"2018-09-22 13:50:23\"" +
                "  }" +
                "}";
            Assert.AreEqual("Vinayaka@mindsol.in", QuickAndDirtyJsonLookupText(json, "email"));

            // a bit more structured - and needed, if more than one user existed in the json:
            string user = QuickAndDirtyJsonFind(json, "User"); // finds the start of the 1'st user
            Assert.AreEqual("43523543", QuickAndDirtyJsonLookupText(user, "ref_id"));
        }
    }
}
