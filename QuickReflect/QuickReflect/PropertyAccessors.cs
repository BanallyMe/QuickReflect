using System;

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
    }
}
