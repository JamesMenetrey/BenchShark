using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Binarysharp.Benchmark.Helpers
{
    /// <summary>
    /// This class provides some tools to perform reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets the tasks from an object by searching for the attribute <see cref="BenchSharkTaskAttribute"/>.
        /// </summary>
        /// <param name="obj">The object where the tasks are retrieved.</param>
        /// <returns>The return value is a collection of the <see cref="Action"/> objects.</returns>
        public static IEnumerable<Action> GetTasksByObject(object obj)
        {
            // Get the methods of the object that have the BenchShark attribute
            var methodsInfo = obj.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                .Where(method => method.GetCustomAttributes(typeof(BenchSharkTaskAttribute), false).Length > 0);

            // For each method decorated with the BenchShark attribute
            foreach (var methodInfo in methodsInfo)
            {
                // Copy the foreach variable to a local variable
                var info = methodInfo;
                // Check that the method doesn't have parameter(s)
                if (info.GetParameters().Any())
                {
                    // Cannot invoke method with parameter(s)
                    throw new Exception("Cannot run the task \"" + info.GetCustomAttribute<BenchSharkTaskAttribute>().Name + "\" because it has parameter(s).");
                }
                // Return a delegate that invoke the method
                yield return () => info.Invoke(obj, null);
            }
        }

        /// <summary>
        /// Gets the tasks from a type by searching for the attribute <see cref="BenchSharkTaskAttribute"/>.
        /// </summary>
        /// <typeparam name="T">The type where the tasks are retrieved.</typeparam>
        /// <returns>The return value is a collection of the <see cref="Action"/> objects.</returns>
        /// <remarks>Calling this method will create a new instance of the given type.</remarks>
        public static IEnumerable<Action> GetTasksByType<T>() where T : new()
        {
            // Create a new instance of the class
            var obj = new T();
            // Get the tasks by its object
            return GetTasksByObject(obj);
        }
    }
}
