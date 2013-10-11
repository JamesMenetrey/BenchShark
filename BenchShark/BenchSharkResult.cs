/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/

using System;

namespace Binarysharp.Benchmark
{
    /// <summary>
    /// This model defines a result of a BenchShark task analysis.
    /// </summary>
    public class BenchSharkResult
    {
        #region Properties
        /// <summary>
        /// The average of all the elapsed ticks for the task.
        /// </summary>
        public long AverageElapsedTicks
        {
            get
            {
                return TotalElapsedTicks / IterationsCount;
            }
        }

        /// <summary>
        /// The average of all the execution time for the task.
        /// </summary>
        public TimeSpan AverageExecutionTime
        {
            get
            {
                return TimeSpan.FromTicks(TotalExecutionTime.Ticks / IterationsCount);
            }
        }

        /// <summary>
        /// The best elapsed ticks for the task.
        /// </summary>
        public long BestElapsedTicks { get; internal set; }

        /// <summary>
        /// The best execution time for the task.
        /// </summary>
        public TimeSpan BestExecutionTime { get; internal set; }

        /// <summary>
        /// The number of iterations performed to compute this result.
        /// </summary>
        public int IterationsCount { get; internal set; }

        /// <summary>
        /// The name of the task.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The total of all the elapsed ticks for the task.
        /// </summary>
        public long TotalElapsedTicks { get; internal set; }

        /// <summary>
        /// The total of all the execution time for the task.
        /// </summary>
        public TimeSpan TotalExecutionTime { get; internal set; }

        /// <summary>
        /// The worst elapsed ticks for the task.
        /// </summary>
        public long WorstElapsedTicks { get; internal set; }

        /// <summary>
        /// The worst execution time for the task.
        /// </summary>
        public TimeSpan WorstExecutionTime { get; internal set; }
        #endregion

        #region Operator Overloading
        /// <summary>
        /// Overloads the addition of two results. Takes the name of the first result.
        /// </summary>
        /// <param name="r1">The first result.</param>
        /// <param name="r2">The second result.</param>
        /// <returns>A merged result.</returns>
        public static BenchSharkResult operator +(BenchSharkResult r1, BenchSharkResult r2)
        {
            return new BenchSharkResult
            {
                // Take the name of the first result
                Name = r1.Name,
                // Sum the iteration counts
                IterationsCount = r1.IterationsCount + r2.IterationsCount,
                // Sum the tick/time
                TotalElapsedTicks = r1.IterationsCount + r2.TotalElapsedTicks,
                TotalExecutionTime = r1.TotalExecutionTime + r2.TotalExecutionTime,
                // Select the best case
                BestElapsedTicks = r1.BestElapsedTicks < r2.BestElapsedTicks
                    ? r1.BestElapsedTicks
                    : r2.BestElapsedTicks,
                BestExecutionTime = r1.BestExecutionTime < r2.BestExecutionTime
                    ? r1.BestExecutionTime
                    : r2.BestExecutionTime,
                // Select the worst case
                WorstElapsedTicks = r1.WorstElapsedTicks > r2.WorstElapsedTicks
                    ? r1.WorstElapsedTicks
                    : r2.WorstElapsedTicks,
                WorstExecutionTime = r1.WorstExecutionTime > r2.WorstExecutionTime
                    ? r1.WorstExecutionTime
                    : r2.WorstExecutionTime
            };
        }
        #endregion
    }
}
