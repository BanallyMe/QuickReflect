using BanallyMe.QuickReflect;
using System;
using Xunit;

namespace QuickReflect_Tests
{
    public class PropertyAccessorsTests
    {
        [Fact]
        public void PropertyAccessors_GetPropertyValue_ContainingObjectIsNull()
        {
            PropertyAccessorsTestClass testObj = null;
            Assert.Throws<ArgumentNullException>(() => PropertyAccessors.GetPropertyValue(testObj, "stringProperty1"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalidProperty")]
        public void PropertyAccessors_GetPropertyValue_PropertyNameIsInvalid(string propertyName)
        {
            var testObj = new PropertyAccessorsTestClass();
            Assert.Throws<ArgumentException>(() => testObj.GetPropertyValue(propertyName));
        }

        [Theory]
        [InlineData("stringProperty1", null)]
        [InlineData("stringProperty2", "abc")]
        [InlineData("stringProperty3", "def")]
        [InlineData("intProperty1", 1)]
        [InlineData("intProperty2", 2)]
        [InlineData("intProperty3", 3)]
        public void PropertyAccessors_GetPropertyValue_ValidPropertyName(string propertyName, object expectedResult)
        {
            var testObj = new PropertyAccessorsTestClass();
            var propertyValue = testObj.GetPropertyValue(propertyName);
            Assert.Equal(propertyValue, expectedResult);
        }

        [Fact]
        public void PropertyAccessors_GetNonNullPropertiesAndValues_ObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => PropertyAccessors.GetNonNullPropertiesAndValues(null));
        }

        [Fact]
        public void PropertyAccessors_GetNonNullPropertiesAndValues_OnlyNulledValues()
        {
            var testObj = new OnlyNullPropertiesTestClass();
            var emptyResultList = testObj.GetNonNullPropertiesAndValues();
            Assert.Empty(emptyResultList);
        }

        [Fact]
        public void PropertyAccessors_GetNonNullPropertiesAndValues_FindsCorrectProperties()
        {
            var testObj = new PropertyAccessorsTestClass();
            var resultPropertyList = testObj.GetNonNullPropertiesAndValues();
            var testClassType = typeof(PropertyAccessorsTestClass);
            Assert.Collection(resultPropertyList,
                first => { Assert.Equal(testClassType.GetProperty("stringProperty2"), first.Property); Assert.Equal("abc",first.CorrespondingValue); },
                second => { Assert.Equal(testClassType.GetProperty("stringProperty3"), second.Property); Assert.Equal("def",second.CorrespondingValue); },
                third => { Assert.Equal(testClassType.GetProperty("intProperty1"), third.Property); Assert.Equal(1,third.CorrespondingValue); },
                fourth => { Assert.Equal(testClassType.GetProperty("intProperty2"), fourth.Property); Assert.Equal(2,fourth.CorrespondingValue); },
                fifth => { Assert.Equal(testClassType.GetProperty("intProperty3"), fifth.Property); Assert.Equal(3,fifth.CorrespondingValue); }
            );
        }

        [Fact]
        public void PropertyAccessors_SetPropertiesByValueType_ContainingObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => PropertyAccessors.SetPropertiesByValueType(null, 1));
        }

        [Fact]
        public void PropertyAccessors_SetPropertiesByValueType_ValueToSetIsNull()
        {
            var testObj = new PropertyAccessorsTestClass();
            Assert.Throws<ArgumentNullException>(() => PropertyAccessors.SetPropertiesByValueType(null, null));
        }

        [Fact]
        public void PropertyAccessors_SetPropertiesByValueType_NonExistingValueType()
        {
            var testObj = new PropertyAccessorsTestClass();
            testObj.SetPropertiesByValueType(true);
            Assert.Null(testObj.stringProperty1);
            Assert.Equal("abc", testObj.stringProperty2);
            Assert.Equal("def", testObj.stringProperty3);
            Assert.Equal(1, testObj.intProperty1);
            Assert.Equal(2, testObj.intProperty2);
            Assert.Equal(3, testObj.intProperty3);
        }

        [Fact]
        public void PropertyAccessors_SetPropertiesByValueType_ReplacesExistingValueType()
        {
            var testObj = new PropertyAccessorsTestClass();
            testObj.SetPropertiesByValueType("ghi");
            Assert.Equal("ghi", testObj.stringProperty1);
            Assert.Equal("ghi", testObj.stringProperty2);
            Assert.Equal("ghi", testObj.stringProperty3);
            Assert.Equal(1, testObj.intProperty1);
            Assert.Equal(2, testObj.intProperty2);
            Assert.Equal(3, testObj.intProperty3);
        }

        private class OnlyNullPropertiesTestClass
        {
            public string stringProperty1 { get; set; } = null;
            public string stringProperty2 { get; set; } = null;
        }

        private class PropertyAccessorsTestClass
        {
            public string stringProperty1 { get; set; } = null;
            public string stringProperty2 { get; set; } = "abc";
            public string stringProperty3 { get; set; } = "def";
            public int intProperty1 { get; set; } = 1;
            public int intProperty2 { get; set; } = 2;
            public int intProperty3 { get; set; } = 3;
        }
    }
}
