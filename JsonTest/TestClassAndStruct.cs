namespace Dbquity.Test {
    public class TestClass {
        private int privateField = 1;
        internal int internalField = 2;
        protected int protectedField = 4;
        internal protected int internalProtectedField = 6;
        public int PublicField = 8;
        private int PrivateAutoGetProperty { get; } = 16 + 1;
        internal int InternalAutoGetProperty { get; } = 16 + 2;
        protected int ProtectedAutoGetProperty { get; } = 16 + 4;
        internal protected int InternalProtectedAutoGetProperty { get; } = 16 + 6;
        public int PublicAutoGetProperty { get; } = 16 + 8;
        private int PrivateAutoGetSetProperty { get; set; } = 32 + 1;
        internal int InternalAutoGetSetProperty { get; set; } = 32 + 2;
        protected int ProtectedAutoGetSetProperty { get; set; } = 32 + 4;
        internal protected int InternalProtectedAutoGetSetProperty { get; set; } = 32 + 6;
        public int PublicAutoGetSetProperty { get; set; } = 32 + 8;
        internal int InternalAutoGetPrivateSetProperty { get; private set; } = 64 + 2;
        protected int ProtectedAutoGetPrivateSetProperty { get; private set; } = 64 + 4;
        internal protected int InternalProtectedAutoGetPrivateSetProperty { get; private set; } = 64 + 6;
        public int PublicAutoGetPrivateSetProperty { get; private set; } = 64 + 8;
        public TestClass() { PrivateAutoGetSetProperty = 32 + privateField; }
    }
    public struct TestStruct {
        private int privateField;
        internal int internalField;
        public int PublicField;
        private int PrivateAutoGetProperty { get; }
        internal int InternalAutoGetProperty { get; }
        public int PublicAutoGetProperty { get; }
        private int PrivateAutoGetSetProperty { get; set; }
        internal int InternalAutoGetSetProperty { get; set; }
        public int PublicAutoGetSetProperty { get; set; }
        internal int InternalAutoGetPrivateSetProperty { get; private set; }
        public int PublicAutoGetPrivateSetProperty { get; private set; }
        public void SetAll() {
            privateField = 1;
            internalField = 2;
            PublicField = 8;
            PrivateAutoGetSetProperty = 32 + privateField;
            InternalAutoGetSetProperty = 32 + 2;
            PublicAutoGetSetProperty = 32 + 8;
            InternalAutoGetPrivateSetProperty = 64 + 2;
            PublicAutoGetPrivateSetProperty = 64 + 8;
        }
    }
}