using System;

namespace Binarysharp.Benchmark
{
    /// <summary>
    /// Represents an event argument when a evaluation is completed.
    /// </summary>
    public class BenchSharkEvaluationEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// The result of a task fully evaluated.
        /// </summary>
        public BenchSharkResult TaskEvaluated { get; protected set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="BenchSharkEvaluationEventArgs"/>.
        /// </summary>
        /// <param name="taskEvaluated">The result of a task fully evaluated.</param>
        public BenchSharkEvaluationEventArgs(BenchSharkResult taskEvaluated)
        {
            TaskEvaluated = taskEvaluated;
        }
        #endregion
    }
}
