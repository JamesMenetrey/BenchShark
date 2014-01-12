/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;

namespace Binarysharp.Benchmark.Components
{
    /// <summary>
    /// Class that optimizes the memory of the current process by running the Garbage Collector.
    /// </summary>
    public class GcMemoryOptimizer : IMemoryOptimizer
    {
        /// <summary>
        /// Optimizes the memory of the current process by forcing a garbage collection.
        /// </summary>
        public void OptimizeMemory()
        {
            // Forces garbage collection
            GC.Collect();
            // Suspends the current thread until the thread that is processing the queue of finalizers has emptied that queue
            GC.WaitForPendingFinalizers();
            // Forces garbage collection one more time to eliminate finalized objects
            GC.Collect();
        }
    }
}
