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
    /// Defines a method that optimizes the memory of the current process.
    /// </summary>
    public interface IMemoryOptimizer
    {
        /// <summary>
        /// Optimizes the memory of the current process.
        /// </summary>
        void OptimizeMemory();
    }
}
