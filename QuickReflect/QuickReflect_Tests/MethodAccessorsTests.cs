using BanallyMe.QuickReflect;
using System;
using Xunit;

namespace QuickReflect_Tests
{
    public class MethodAccessorsTests
    {
        [Fact]
        public void MethodAccessors_InvokeActionMethod_ContainingObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => MethodAccessors.InvokeActionMethod(null, "anyMethod", null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalidMethodName")]
        public void MethodAccessors_InvokeActionMethod_MethodDoesNotExist(string invalidMethodName)
        {
            var testObj = new MethodAccessorsTestClass();
            Assert.Throws<ArgumentException>(() => testObj.InvokeActionMethod(invalidMethodName, null));
        }

        [Fact]
        public void MethodAccessors_InvokeActionMethod_MethodWithoutParamsIsCalled()
        {
            var testObj = new MethodAccessorsTestClass();
            testObj.InvokeActionMethod(nameof(MethodAccessorsTestClass.IncreaseCalls), null);
            Assert.Equal(1, testObj.method1Called);
        }

        [Fact]
        public void MethodAccessors_InvokeActionMethod_MethodWithParamsIsCalled()
        {
            var testObj = new MethodAccessorsTestClass();
            testObj.InvokeActionMethod(nameof(MethodAccessorsTestClass.IncreaseCallsByIncrement), new object[] { 3 });
            Assert.Equal(3, testObj.method1Called);
        }

        [Fact]
        public void MethodAccessors_InvokeFuncMethod_ContainingObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => MethodAccessors.InvokeFuncMethod(null, "anyMethod", null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalidMethodName")]
        public void MethodAccessors_InvokeFuncMethod_MethodDoesNotExist(string invalidMethodName)
        {
            var testObj = new MethodAccessorsTestClass();
            Assert.Throws<ArgumentException>(() => testObj.InvokeFuncMethod(invalidMethodName, null));
        }

        [Fact]
        public void MethodAccessors_InvokeFuncMethod_MethodWithoutParamsIsCalled()
        {
            var testObj = new MethodAccessorsTestClass();
            var funcResult = testObj.InvokeFuncMethod(nameof(MethodAccessorsTestClass.Return42), null);
            Assert.Equal(42, funcResult);
        }

        [Fact]
        public void MethodAccessors_InvokeFuncMethod_MethodWithParamsIsCalled()
        {
            var testObj = new MethodAccessorsTestClass();
            var funcResult = testObj.InvokeFuncMethod(nameof(MethodAccessorsTestClass.Return42TimesFactor), new object[] { 3 });
            Assert.Equal(126, funcResult);
        }

        [Fact]
        public void MethodAccessors_InvokeFuncMethodT_ContainingObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => MethodAccessors.InvokeFuncMethod<int>(null, "anyMethod", null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalidMethodName")]
        public void MethodAccessors_InvokeFuncMethodT_MethodDoesNotExist(string invalidMethodName)
        {
            var testObj = new MethodAccessorsTestClass();
            Assert.Throws<ArgumentException>(() => testObj.InvokeFuncMethod<int>(invalidMethodName, null));
        }

        [Fact]
        public void MethodAccessors_InvokeFuncMethodT_InvalidReturnType()
        {
            var testObj = new MethodAccessorsTestClass();
            Assert.Throws<InvalidCastException>(() => testObj.InvokeFuncMethod<string>(nameof(MethodAccessorsTestClass.Return42), null));
        }

        [Fact]
        public void MethodAccessors_InvokeFuncMethodT_MethodWithoutParamsIsCalled()
        {
            var testObj = new MethodAccessorsTestClass();
            var funcResult = testObj.InvokeFuncMethod<int>(nameof(MethodAccessorsTestClass.Return42), null);
            Assert.StrictEqual(42, funcResult);
        }

        [Fact]
        public void MethodAccessors_InvokeFuncMethodT_MethodWithParamsIsCalled()
        {
            var testObj = new MethodAccessorsTestClass();
            var funcResult = testObj.InvokeFuncMethod<int>(nameof(MethodAccessorsTestClass.Return42TimesFactor), new object[] { 3 });
            Assert.StrictEqual(126, funcResult);
        }

        private class MethodAccessorsTestClass
        {
            public int method1Called { get; private set; } = 0;

            public void IncreaseCalls()
            {
                method1Called++;
            }

            public void IncreaseCallsByIncrement(int increment)
            {
                method1Called += increment;
            }

            public int Return42()
            {
                return 42;
            }

            public Int32 Return42TimesFactor(int factor)
            {
                return factor * 42;
            }
        }
    }
}
