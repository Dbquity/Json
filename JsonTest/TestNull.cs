using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dbquity.Test {
    [TestClass]
    public class TestNull {
        [TestMethod]
        public void ImplicitCastsFromNull() {
            JsonValue value = null;
            Assert.AreEqual<string>(null, value);
            Assert.AreEqual<bool>(false, value);
            Assert.AreEqual<int>(0, value);
        }
    }
}
