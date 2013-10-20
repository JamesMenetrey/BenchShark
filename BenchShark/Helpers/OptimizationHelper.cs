/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;
using System.Diagnostics;

namespace Binarysharp.Benchmark.Helpers
{
    /// <summary>
    /// Provides useful functioons related to optimize the process.
    /// </summary>
    public static class OptimizationHelper
    {
        #region IsBeingDebugged
        /// <summary>
        /// Gets a value that indicates whether a debugger is attached to the process.
        /// </summary>
        public static bool IsBeingDebugged
        {
            get
            {
                return Debugger.IsAttached;
            }
        }
        #endregion

        #region IsInDebugMode
        /// <summary>
        /// Gets a value that indicates whether the project is compiled under the Debug mode.
        /// </summary>
        public static bool IsInDebugMode
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
        #endregion

        #region IsOptimizedProcess
        /// <summary>
        /// Gets a value that indicates whether the process is optimized.
        /// </summary>
        public static bool IsOptimizedProcess
        {
            get
            {
                return !IsBeingDebugged && !IsInDebugMode;
            }
        }
        #endregion

        #region OptimizeMemory
        /// <summary>
        /// Optimizes the memory of the current process, avoiding the Garbage Collector to be called afterwards.
        /// </summary>
        public static void OptimizeMemory()
        {
            // Forces garbage collection
            GC.Collect();
            // Suspends the current thread until the thread that is processing the queue of finalizers has emptied that queue
            GC.WaitForPendingFinalizers();
            // Forces garbage collection one more time to eliminate finalized objects
            GC.Collect();
        }
        #endregion
    }
}
