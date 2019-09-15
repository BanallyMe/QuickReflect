using System.Reflection;

namespace BanallyMe.QuickReflect.Models
{
    /// <summary>
    /// Pair of an object Property and its corresponding value on an object.
    /// </summary>
    public class PropertyValuePair
    {
        /// <summary>
        /// The object's property.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// The value of this property on a certain object.
        /// </summary>
        public object CorrespondingValue { get; }

        /// <summary>
        /// Creates a new PropertyValuePair.
        /// </summary>
        /// <param name="property">An object's property.</param>
        /// <param name="correspondingValue">The value of this property on a certain object.</param>
        public PropertyValuePair(PropertyInfo property, object correspondingValue)
        {
            Property = property;
            CorrespondingValue = correspondingValue;
        }

        /// <summary>
        /// Creates a new PropertyValuePair from an existing object and one of its properties.
        /// </summary>
        /// <param name="containingObject">Object to be read.</param>
        /// <param name="property">Property to read the corresponding Value from.</param>
        /// <returns>A Pair of the property and its corresponding value.</returns>
        public static PropertyValuePair CreateFromObjectsProperty(object containingObject, PropertyInfo property)
        {
            return new PropertyValuePair(property, property.GetValue(containingObject));
        }
    }
}
