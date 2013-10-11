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
    }
}
