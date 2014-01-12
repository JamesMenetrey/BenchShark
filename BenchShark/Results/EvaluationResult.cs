/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;
using System.Collections.Generic;

namespace Binarysharp.Benchmark.Results
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
        /// <param name="name">The name of the evaluation.</param>
        /// <param name="mustStoreIterations">
        /// Determines whether the evaluation must store the result of each iteration (does not alter the exposed properties).
        /// If this value is set to <c>true</c>, the evaluation can be memory consuming, depending on the number of iterations.
        /// </param>
        internal EvaluationResult(string name, bool mustStoreIterations = false)
        {
            InternalIterations = new List<IterationResult>();
            Name = name;
            MustStoreIterations = mustStoreIterations;
        }
        #endregion

        #region Methods
        #region AddIteration
        /// <summary>
        /// Adds the iteration to the evaluation result.
        /// </summary>
        /// <param name="iteration">The iteration to add.</param>
        internal void AddIteration(IterationResult iteration)
        {
            // Store the iteration is the feature is enabled
            if (MustStoreIterations)
            {
                InternalIterations.Add(iteration);
            }

            
            // Sum the tick/time
            TotalElapsedTicks += iteration.ElapsedTicks;
            TotalExecutionTime += iteration.ExecutionTime;
            // Select the best/worst case
            // Set the value of the iteration if this is the first one
            if (IterationsCount == 0)
            {
                BestElapsedTicks = iteration.ElapsedTicks;
                BestExecutionTime = iteration.ExecutionTime;
                WorstElapsedTicks = iteration.ElapsedTicks;
                WorstExecutionTime = iteration.ExecutionTime;
            }
            else
            {
                BestElapsedTicks = iteration.ElapsedTicks < BestElapsedTicks
                    ? iteration.ElapsedTicks
                    : BestElapsedTicks;
                BestExecutionTime = iteration.ExecutionTime < BestExecutionTime
                    ? iteration.ExecutionTime
                    : BestExecutionTime;
                WorstElapsedTicks = iteration.ElapsedTicks > WorstElapsedTicks
                    ? iteration.ElapsedTicks
                    : WorstElapsedTicks;
                WorstExecutionTime = iteration.ExecutionTime > WorstExecutionTime
                    ? iteration.ExecutionTime
                    : WorstExecutionTime;
            }
            // Increment the number of iteration
            IterationsCount++;
        }
        #endregion
        #endregion
    }
}
