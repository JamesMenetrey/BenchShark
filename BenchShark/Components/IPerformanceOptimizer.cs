/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
namespace Binarysharp.Benchmark.Components
{
    /// <summary>
    /// Defines a set of methods that creates a timeframe during which the performance of the current process is optimized.
    /// </summary>
    public interface IPerformanceOptimizer
    {
        /// <summary>
        /// Enters a section during which the performance of the current process is optimized.
        /// </summary>
        void EnterOptimizedSection();

        /// <summary>
        /// Leaves a section during which the performance of the current process is optimized.
        /// </summary>
        void LeaveOptimizedSection();
    }
}
