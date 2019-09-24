using System;
using System.Linq;
using System.Reflection;

namespace BanallyMe.QuickReflect.Models
{
    public class PropertyAttributesPair<TAttribute> where TAttribute : Attribute
    {
        /// <summary>
        /// The object's property.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// An Array of Attributes of type TAttribute on this property.
        /// </summary>
        public TAttribute[] Attributes { get; }

        /// <summary>
        /// Creates a new PropertyAttributesPair.
        /// </summary>
        /// <param name="property">An object's property.</param>
        /// <param name="attributes">An Array of attributes connected to the property.</param>
        public PropertyAttributesPair(PropertyInfo property, TAttribute[] attributes)
        {
            Property = property;
            Attributes = attributes;
        }

        /// <summary>
        /// Creates a collection of PropertyAttributesPair from an existing object.
        /// </summary>
        /// <param name="containingObject">Object to be read.</param>
        /// <returns>A Pair of the property and its corresponding value.</returns>
        public static PropertyAttributesPair<TAttribute>[] CreateFromObjectsProperty(object containingObject)
        {
            return containingObject.GetType()
                .GetProperties()
                .Select(prop => new PropertyAttributesPair<TAttribute>(prop, prop.GetCustomAttributes<TAttribute>().ToArray()))
                .Where(pair => pair.Attributes.Length > 0)
                .ToArray();
        }
    }
}
