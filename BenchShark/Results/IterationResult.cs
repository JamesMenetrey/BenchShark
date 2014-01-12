/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;

namespace Binarysharp.Benchmark.Results
{
    /// <summary>
    /// The result of an iteration completed by BenchShark.
    /// </summary>
    public class IterationResult
    {
        #region Properties
        /// <summary>
        /// The execution time for the iteration.
        /// </summary>
        public TimeSpan ExecutionTime { get; protected set; }

        /// <summary>
        /// The elapsed ticks for the iteration.
        /// </summary>
        public long ElapsedTicks { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IterationResult"/> class.
        /// </summary>
        /// <param name="executionTime">The execution time for the iteration.</param>
        /// <param name="elapsedTicks">The elapsed ticks for the iteration.</param>
        internal IterationResult(TimeSpan executionTime, long elapsedTicks)
        {
            ExecutionTime = executionTime;
            ElapsedTicks = elapsedTicks;
        }
        #endregion
    }
}
