using System;

namespace Binarysharp.Benchmark
{
    /// <summary>
    /// Represents an event argument when an evaluation is performed.
    /// </summary>
    public class BenchSharkEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// The result of the evaluation.
        /// </summary>
        public BenchSharkResult Result { get; protected set; }

        /// <summary>
        /// The total of the results of the evaluation.
        /// </summary>
        public BenchSharkResult TotalResults { get; protected set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="BenchSharkEventArgs"/>.
        /// </summary>
        /// <param name="result">The result of an evaluation.</param>
        /// <param name="totalResults">The total of the results of the evaluation.</param>
        public BenchSharkEventArgs(BenchSharkResult result, BenchSharkResult totalResults)
        {
            Result = result;
            TotalResults = totalResults;
        }
        #endregion
    }
}
