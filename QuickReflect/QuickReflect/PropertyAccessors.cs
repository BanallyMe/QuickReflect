using BanallyMe.QuickReflect.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanallyMe.QuickReflect
{
    /// <summary>
    /// Provides some shortcuts for accessing properties of an object via reflection.
    /// </summary>
    public static class PropertyAccessors
    {
        /// <summary>
        /// Returns the value of the property with the given name from a specific object.
        /// </summary>
        /// <param name="containingObject">Object the value will be retrieved of.</param>
        /// <param name="propertyName">Property whose value will be retrieved.</param>
        /// <returns>The value of the given property.</returns>
        /// <exception cref="ArgumentNullException">Thrown if containingObject is null.</exception>
        /// <exception cref="ArgumentException">Thrown if there is no such property on the given object.</exception>
        public static object GetPropertyValue(this object containingObject, string propertyName)
        {
            if (containingObject == null)
            {
                throw new ArgumentNullException(nameof(containingObject),"Cannot access property on a null object.");
            }

            var containingObjectType = containingObject.GetType();
            var accessedProperty = containingObjectType.GetProperty(propertyName);
            if (accessedProperty == null)
            {
                throw new ArgumentException($"There is no property named '{propertyName}' on object of type '{containingObjectType.AssemblyQualifiedName}'.");
            }

            return accessedProperty.GetValue(containingObject);
        }

        /// <summary>
        /// Sets a given value on all properties of an object, that have the same type as the value that should be set.
        /// </summary>
        /// <param name="containingObject">Object whose properties are being set.</param>
        /// <param name="valueToSet">Value that will be set on the object's properties.</param>
        /// <exception cref="ArgumentNullException">Thrown if containingObject or valueToSet are null.</exception>
        public static void SetPropertiesByValueType(this object containingObject, object valueToSet)
        {
            if (containingObject == null)
            {
                throw new ArgumentNullException(nameof(containingObject), "Cannot read properties of a null object.");
            }

            if (valueToSet == null)
            {
                throw new ArgumentNullException(nameof(valueToSet), $"Cannot set values to null via the '{nameof(SetPropertiesByValueType)}'-method");
            }
            var valueType = valueToSet.GetType();
            var propertiesToSet = containingObject
                .GetType()
                .GetProperties()
                .Where(property => property.PropertyType == valueType);
            Parallel.ForEach(propertiesToSet, property => property.SetValue(containingObject, valueToSet));
        }

        /// <summary>
        /// Read all properties and values from an object where the property's value is not null.
        /// </summary>
        /// <param name="objectToRead">Object to read properties and values from.</param>
        /// <returns>Array of all properties and their coresponding values where the values are not null.</returns>
        public static PropertyValuePair[] GetNonNullPropertiesAndValues(object objectToRead)
        {
            if (objectToRead == null)
            {
                throw new ArgumentNullException(nameof(objectToRead), "Cannot read properties from a null object.");
            }

            return objectToRead.GetType()
                .GetProperties()
                .Where(property => property.GetValue(objectToRead) != null)
                .Select(property => PropertyValuePair.CreateFromObjectsProperty(objectToRead, property))
                .ToArray();
        }
    }
}
