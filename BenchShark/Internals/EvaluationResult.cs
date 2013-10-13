/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;
using System.Collections.Generic;

namespace Binarysharp.Benchmark.Internals
{
    /// <summary>
    /// The result of an evaluation completed by BenchShark.
    /// </summary>
    public class EvaluationResult
    {
        #region Fields
        /// <summary>
        /// The collection containing the iterations.
        /// This field is only filled when the value <see cref="MustStoreIterations"/> is set to <c>true</c>.
        /// </summary>
        protected List<IterationResult> InternalIterations;

        /// <summary>
        /// Determines whether the result must store all the iterations.
        /// If this value is set to <c>true</c>, the evaluation can be memory consuming, depending on the number of iterations.
        /// </summary>
        protected bool MustStoreIterations;
        #endregion

        #region Properties
        /// <summary>
        /// The average of all the elapsed ticks for the evaluation.
        /// </summary>
        public long AverageElapsedTicks
        {
            get
            {
                return TotalElapsedTicks / IterationsCount;
            }
        }

        /// <summary>
        /// The average of all the execution time for the evaluation.
        /// </summary>
        public TimeSpan AverageExecutionTime
        {
            get
            {
                return TimeSpan.FromTicks(TotalExecutionTime.Ticks / IterationsCount);
            }
        }

        /// <summary>
        /// The best elapsed ticks for the evaluation.
        /// </summary>
        public long BestElapsedTicks { get; protected set; }

        /// <summary>
        /// The best execution time for the evaluation.
        /// </summary>
        public TimeSpan BestExecutionTime { get; protected set; }

        /// <summary>
        /// The collection containing the iterations.
        /// This property can only be queried when the value <see cref="MustStoreIterations"/> is set to <c>true</c>.
        /// </summary>
        public IEnumerable<IterationResult> Iterations
        {
            get
            {
                // Check if the object stores the iterations
                if (!MustStoreIterations)
                {
                    throw new Exception("The result does not contain the iterations. Create the instance of the object with the parameter 'mustStoreIterations' set to true.");
                }
                return InternalIterations.AsReadOnly();
            }
        }

        /// <summary>
        /// The number of iterations for the evaluation.
        /// </summary>
        public int IterationsCount { get; protected set; }

        /// <summary>
        /// The name of the task.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The total of all the elapsed ticks for the evaluation.
        /// </summary>
        public long TotalElapsedTicks { get; protected set; }

        /// <summary>
        /// The total of all the execution time for the evaluation.
        /// </summary>
        public TimeSpan TotalExecutionTime { get; protected set; }

        /// <summary>
        /// The worst elapsed ticks for the evaluation.
        /// </summary>
        public long WorstElapsedTicks { get; protected set; }

        /// <summary>
        /// The worst execution time for the evaluation.
        /// </summary>
        public TimeSpan WorstExecutionTime { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationResult"/> class.
        /// </summary>
        /// <param name="mustStoreIterations">
        /// Determines whether the result must store all the iterations.
        /// If this value is set to <c>true</c>, the evaluation can be memory consuming, depending on the number of iterations.
        /// </param>
        internal EvaluationResult(bool mustStoreIterations)
        {
            MustStoreIterations = mustStoreIterations;
        }
        #endregion

        #region Methods
        #region AddIteration
        /// <summary>
        /// Adds the iteration to the evaluation result.
        /// </summary>
        /// <param name="iteration">The iteration to add.</param>
        public void AddIteration(IterationResult iteration)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Operators Overloading
        /// <summary>
        /// Overloads the addition of two evaluation results. Takes the name of the first evaluation.
        /// </summary>
        /// <param name="r1">The first evaluation result.</param>
        /// <param name="r2">The second evaluation result.</param>
        /// <returns>A merged evaluation result.</returns>
        public static BenchSharkResult operator +(EvaluationResult r1, EvaluationResult r2)
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
        #endregion
    }
}
