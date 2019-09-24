using System;

namespace BanallyMe.QuickReflect
{
    /// <summary>
    /// Provides some shortcuts for accessing properties of an object via reflection.
    /// </summary>
    public static class MethodAccessors
    {
        /// <summary>
        /// Invokes a method with the given name on the given object.
        /// The method has no return value.
        /// </summary>
        /// <param name="containingObject">Object whose method is invoked.</param>
        /// <param name="methodName">Name of the invoked method.</param>
        /// <param name="methodParameters">Parameters passed to the invoked method.</param>
        /// <exception cref="ArgumentNullException">Thrown if the containingObject is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the containingObject does not contain a method with the given name.</exception>
        public static void InvokeActionMethod(this object containingObject, string methodName, object[] methodParameters)
        {
            containingObject.InvokeFuncMethod(methodName, methodParameters);
        }

        /// <summary>
        /// Invokes a method with the given name on the given object and returns the method's return value.
        /// </summary>
        /// <param name="containingObject">Object whose method is invoked.</param>
        /// <param name="methodName">Name of the invoked method.</param>
        /// <param name="methodParameters">Parameters passed to the invoked method.</param>
        /// <returns>Return value of the invoked method.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the containingObject is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the containingObject does not contain a method with the given name.</exception>
        public static object InvokeFuncMethod(this object containingObject, string methodName, object[] methodParameters)
        {
            if (containingObject == null)
            {
                throw new ArgumentNullException(nameof(containingObject), "A method of a null reference cannot be invoked.");
            }
            if(methodName == null)
            {
                throw new ArgumentException($"A Method without a name cannot be invoked ({nameof(methodName)} was null).");
            }
            var containingObjectType = containingObject.GetType();
            var methodToInvoke = containingObjectType.GetMethod(methodName);
            if (methodToInvoke == null)
            {
                throw new ArgumentException($"Method '{methodName}' does not exist on object of type {containingObjectType.AssemblyQualifiedName}");
            }

            return methodToInvoke.Invoke(containingObject, methodParameters);
        }

        /// <summary>
        /// Invokes a method with the given name on the given object and returns the method's typed return value.
        /// </summary>
        /// <typeparam name="TReturnValue">Type of the method's return value.</typeparam>
        /// <param name="containingObject">Object whose method is invoked.</param>
        /// <param name="methodName">Name of the invoked method.</param>
        /// <param name="methodParameters">Parameters passed to the invoked method.</param>
        /// <returns>Typed return value of the invoked method.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the containingObject is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the containingObject does not contain a method with the given name.</exception>
        /// <exception cref="InvalidCastException">Thrown if the invoked method does not return a value of the given return type.</exception>
        public static TReturnValue InvokeFuncMethod<TReturnValue>(this object containingObject, string methodName, object[] methodParameters)
        {
            try
            {
                return (TReturnValue) containingObject.InvokeFuncMethod(methodName, methodParameters);
            }
            catch (InvalidCastException exception)
            {
                throw new InvalidCastException($"Method '{methodName}' of object of type '{containingObject.GetType().AssemblyQualifiedName}' does not return a value of type '{typeof(TReturnValue).AssemblyQualifiedName}'", exception);
            }
        }
    }
}
