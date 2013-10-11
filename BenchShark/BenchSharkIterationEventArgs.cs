using System;

namespace Binarysharp.Benchmark
{
    /// <summary>
    /// Represents an event argument when an iteration is completed.
    /// </summary>
    public class BenchSharkIterationEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// The result of the current iteration of the evaluation.
        /// </summary>
        public BenchSharkResult CurrentIteration { get; protected set; }

        /// <summary>
        /// The result of the current running evaluation.
        /// </summary>
        public BenchSharkResult CurrentEvaluation { get; protected set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="BenchSharkIterationEventArgs"/>.
        /// </summary>
        /// <param name="currentIteration">The current iteration of the evaluation.</param>
        /// <param name="currentEvaluation">The current running evaluation.</param>
        public BenchSharkIterationEventArgs(BenchSharkResult currentIteration, BenchSharkResult currentEvaluation)
        {
            CurrentIteration = currentIteration;
            CurrentEvaluation = currentEvaluation;
        }
        #endregion
    }
}
