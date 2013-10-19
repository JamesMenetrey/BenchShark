using System;
using System.Collections.Generic;
using System.Linq;

namespace Binarysharp.Benchmark.Helpers
{
    /// <summary>
    /// This class provides some tools to perform reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        public static IEnumerable<Action> GetTasksByObject(object obj)
        {
            // Get the methods of the object that have the BenchShark attribute
            var methodsInfo = obj.GetType()
                .GetMethods()
                .Where(method => method.GetCustomAttributes(typeof(BenchSharkTaskAttribute), false).Length > 0);

            // Return delegates that invoke the methods
            foreach (var methodInfo in methodsInfo)
            {
                var info = methodInfo;
                yield return () => info.Invoke(obj, null);
            }
        }
    }
}
