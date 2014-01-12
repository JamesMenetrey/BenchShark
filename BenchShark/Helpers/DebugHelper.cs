/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System.Diagnostics;

namespace Binarysharp.Benchmark.Helpers
{
    /// <summary>
    /// Provides static methods related to debugging.
    /// </summary>
    public static class DebuggingHelper
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

    }
}
