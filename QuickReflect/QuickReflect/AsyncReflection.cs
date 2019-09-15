using System;
using System.Threading.Tasks;

namespace BanallyMe.QuickReflect
{
    /// <summary>
    /// Provides Shortcuts for directly accessing Object methods asynchronously.
    /// </summary>
    public static class AsyncReflection
    {
        /// <summary>
        /// Invokes and awaits a method with the given name on the given object with the given parameters.
        /// </summary>
        /// <param name="containingObject">Object whose method is going to be invoked.</param>
        /// <param name="methodName">Name of the method that should be invoked.</param>
        /// <param name="methodParameters">Array of parameters passed to the invoked method.</param>
        /// <returns>The task of the method invokation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the containingObject is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the containingObject does not contain a method with the given name.</exception>
        /// <exception cref="InvalidCastException">Thrown if the invoked method does not return a task.</exception>
        public static async Task InvokeActionMethodAsync(this object containingObject, string methodName, object[] methodParameters)
        {
            await containingObject.InvokeFuncMethod<Task>(methodName, methodParameters);
        }

        /// <summary>
        /// Invokes and awaits a method with the given name on the given object with the given parameters and returns the task's result.
        /// </summary>
        /// <param name="containingObject">Object whose method is going to be invoked.</param>
        /// <param name="methodName">Name of the method that should be invoked.</param>
        /// <param name="methodParameters">Array of parameters passed to the invoked method.</param>
        /// <returns>The task of the method invokation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the containingObject is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the containingObject does not contain a method with the given name.</exception>
        /// <exception cref="InvalidCastException">Thrown if the invoked method does not return a task.</exception>
        public static async Task<object> InvokeFuncMethodAsync(this object containingObject, string methodName, object[] methodParameters)
        {
            var invokationTask = containingObject.InvokeFuncMethod<Task>(methodName, methodParameters);
            await invokationTask;

            return GetTaskResult(invokationTask);
        }

        /// <summary>
        /// Invokes and awaits a method with the given name on the given object with the given parameters and returns the task's result.
        /// </summary>
        /// <typeparam name="TTaskReturnValue">Type of the task's result.</typeparam>
        /// <param name="containingObject">Object whose method is going to be invoked.</param>
        /// <param name="methodName">Name of the method that should be invoked.</param>
        /// <param name="methodParameters">Array of parameters passed to the invoked method.</param>
        /// <returns>The task of the method invokation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the containingObject is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the containingObject does not contain a method with the given name.</exception>
        /// <exception cref="InvalidCastException">Thrown if the invoked method does not return a task or the task's result is not of type TTaskReturnValue.</exception>
        public static async Task<TTaskReturnValue> InvokeFuncMethodAsync<TTaskReturnValue>(this object containingObject, string methodName, object[] methodParameters)
        {
            try
            {
                return (TTaskReturnValue) await containingObject.InvokeFuncMethodAsync(methodName, methodParameters);
            }
            catch (InvalidCastException exception)
            {
                throw new InvalidCastException($"", exception);
            }
        }

        /// <summary>
        /// Gets the result of a given Task using reflection. Can be used, if Task cannot be
        /// casted as an instance of Task&lt;T&gt;
        /// </summary>
        /// <param name="task">Task, whose Result will be retrieved.</param>
        /// <returns>Result of the given Task.</returns>
        /// <exception cref="ArgumentNullException">The given task is null.</exception>
        /// <exception cref="ArgumentException">The given task doesn't have a Result.</exception>
        public static object GetTaskResult(Task task)
        {
            return task.GetPropertyValue("Result");
        }

        /// <summary>
        /// Gets the result of a given Task using reflection. Can be used, if Task cannot be
        /// casted as an instance of Task&lt;T&gt;
        /// </summary>
        /// <param name="task">Task, whose Result will be retrieved.</param>
        /// <returns>Result of the given Task.</returns>
        /// <exception cref="ArgumentNullException">The given task is null.</exception>
        /// <exception cref="ArgumentException">The given task doesn't have a Result.</exception>
        /// <exception cref="InvalidCastException">The result's task is not of type TReturnValue.</exception>
        public static TReturnValue GetTaskResult<TReturnValue>(Task task)
        {
            try
            {
                return (TReturnValue) GetTaskResult(task);
            }
            catch (InvalidCastException exception)
            {
                throw new InvalidCastException($"The result task's result is not of type '{typeof(TReturnValue).AssemblyQualifiedName}'.", exception);
            }
        }
    }
}
