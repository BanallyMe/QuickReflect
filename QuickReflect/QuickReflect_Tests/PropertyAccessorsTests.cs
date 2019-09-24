using BanallyMe.QuickReflect;
using BanallyMe.QuickReflect.Models;
using System;
using System.Linq;
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

        [Fact]
        public void PropertyAccessors_GetPropertiesWithAttributes_ContainingObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => PropertyAccessors.GetPropertiesWithAttribute<TestAttribute>(null));
        }

        [Fact]
        public void PropertyAccessors_GetPropertiesWithAttributes_ClassWithoutAttributes()
        {
            var testObj = new PropertyAccessorsTestClass();
            var propertyAttributesPairs = testObj.GetPropertiesWithAttribute<TestAttribute>();
            Assert.Empty(propertyAttributesPairs);
        }

        [Fact]
        public void PropertyAccessors_GetPropertiesWithAttributes_ClassWithSingleAttributes()
        {
            Test_GetPropertiesWithAttributes_UsingTestClass<AttributesContainingTestClass>(
                new PropertyAttributeValues(nameof(AttributesContainingTestClass.stringProperty1), new[] { 1 }),
                new PropertyAttributeValues(nameof(AttributesContainingTestClass.intProperty1), new[] { 2 })
            );
        }

        [Fact]
        public void PropertyAccessors_GetPropertiesWithAttributes_ClassWithMultipleAttributes()
        {
            Test_GetPropertiesWithAttributes_UsingTestClass<MultipleAttributesContainingTestClass>(
                new PropertyAttributeValues(nameof(AttributesContainingTestClass.stringProperty1), new[] { 1, 2 }),
                new PropertyAttributeValues(nameof(AttributesContainingTestClass.intProperty1), new[] { 3 }),
                new PropertyAttributeValues(nameof(AttributesContainingTestClass.intProperty3), new[] { 4, 5, 6 })
            );
        }
        
        private void Test_GetPropertiesWithAttributes_UsingTestClass<TTestClass>(params PropertyAttributeValues[] expectedPairs)
        {
            var testObj = Activator.CreateInstance<TTestClass>();
            var propertyAttributesPairs = testObj.GetPropertiesWithAttribute<TestAttribute>();
            Assert.Collection(propertyAttributesPairs, AssertPropertyAttributePairs(expectedPairs));
        }


        private Action<PropertyAttributesPair<TestAttribute>>[] AssertPropertyAttributePairs(params PropertyAttributeValues[] expectedPairs)
        {
            return expectedPairs
                .Select<PropertyAttributeValues, Action<PropertyAttributesPair<TestAttribute>>>(pair => delegate (PropertyAttributesPair<TestAttribute> propAttrPair)
                {
                    Assert.Equal(pair.PropertyName, propAttrPair.Property.Name);
                    Assert.Collection(propAttrPair.Attributes, AssertPropertyAttributes(pair.AttributeValues));
                })
                .ToArray();
        }

        private Action<TestAttribute>[] AssertPropertyAttributes(params int[] values)
        {
            return values
                .Select<int, Action<TestAttribute>>(value => delegate (TestAttribute attribute) { Assert.Equal(value, attribute.TestValue); })
                .ToArray();
        }        

        private class PropertyAttributeValues
        {
            public PropertyAttributeValues(string propertyName, int[] attributeValues)
            {
                PropertyName = propertyName;
                AttributeValues = attributeValues;
            }

            public string PropertyName { get; set; }
            public int[] AttributeValues { get; set; }
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

        private class AttributesContainingTestClass
        {
            [Test(1)]
            public string stringProperty1 { get; set; } = null;
            public string stringProperty2 { get; set; } = "abc";
            public string stringProperty3 { get; set; } = "def";
            [Test(2)]
            public int intProperty1 { get; set; } = 1;
            public int intProperty2 { get; set; } = 2;
            public int intProperty3 { get; set; } = 3;
        }

        private class MultipleAttributesContainingTestClass
        {
            [Test(1)]
            [Test(2)]
            public string stringProperty1 { get; set; } = null;
            public string stringProperty2 { get; set; } = "abc";
            public string stringProperty3 { get; set; } = "def";
            [Test(3)]
            public int intProperty1 { get; set; } = 1;
            public int intProperty2 { get; set; } = 2;
            [Test(4)]
            [Test(5)]
            [Test(6)]
            public int intProperty3 { get; set; } = 3;
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        private class TestAttribute : Attribute
        {
            public TestAttribute(int testValue)
            {
                TestValue = testValue;
            }

            public int TestValue { get; }
        }
    }
}
