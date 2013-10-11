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
using System.Diagnostics;
using System.Linq;

namespace Binarysharp.Benchmark
{
    /// <summary>
    /// Class to evaluate the performance of functions.
    /// </summary>
    public class BenchShark
    {
        #region Protected Fields
        /// <summary>
        /// The number of interval of iteration to perform a memory clean up.
        /// </summary>
        protected uint CleanUpInterval;

        /// <summary>
        /// The tasks to evaluate.
        /// </summary>
        protected Dictionary<string, Action> Tasks { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// This event signals when an iteration for a task is completed.
        /// </summary>
        public event EventHandler<BenchSharkIterationEventArgs> IterationCompleted;

        /// <summary>
        /// This event signals when an evaluation for a task is completed.
        /// </summary>
        public event EventHandler<BenchSharkEvaluationEventArgs> EvaluationCompleted;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class <see cref="BenchShark"/>.
        /// </summary>
        /// <param name="cleanUpInterval">The number of interval of iteration to perform a memory clean up.</param>
        public BenchShark(uint cleanUpInterval = 1000)
        {
            CleanUpInterval = cleanUpInterval;
            Tasks = new Dictionary<string, Action>();
        }
        #endregion

        #region Protected Methods
        #region CleanUpMemory
        /// <summary>
        /// Cleans up the memory of the current process, avoiding the Garbage Collector to be called afterwards.
        /// </summary>
        protected void CleanUpMemory()
        {
            // Collect all the unused objects
            GC.Collect();
            // Wait on the objects which are finalizing
            GC.WaitForPendingFinalizers();
        }
        #endregion
        #region InternalEvaluateTask
        /// <summary>
        /// Evaluate the performance of the task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="task">The task to evaluate.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        protected BenchSharkResult InternalEvaluateTask(string name, Action task)
        {
            // Initialize the stopwatch
            var watch = new Stopwatch();
            // Start the evaluation of the task
            watch.Start();
            // Run the task
            task();
            // Stop the evaluation
            watch.Stop();

            // Create the return value
            var ret = new BenchSharkResult
            {
                IterationsCount = 1,
                Name = name
            };
            // Set the evaluated ticks
            ret.BestElapsedTicks = ret.TotalElapsedTicks = ret.WorstElapsedTicks = watch.ElapsedTicks;
            // Set the evaluated time
            ret.BestExecutionTime = ret.TotalExecutionTime = ret.WorstExecutionTime = watch.Elapsed;

            // Return the object
            return ret;
        }
        #endregion
        #region OnEvaluationCompleted
        /// <summary>
        /// Raises the event <see cref="EvaluationCompleted"/>, stating that an evaluation was performed.
        /// </summary>
        /// <param name="taskEvaluated">The result of a task fully evaluated.</param>
        protected virtual void OnEvaluationCompleted(BenchSharkResult taskEvaluated)
        {
            if (EvaluationCompleted != null)
            {
                EvaluationCompleted(this, new BenchSharkEvaluationEventArgs(taskEvaluated));
            }
        }
        #endregion
        #region OnIterationCompleted
        /// <summary>
        /// Raises the event <see cref="IterationCompleted"/>, stating that an iteration was performed.
        /// </summary>
        /// <param name="currentIteration">The current iteration of the evaluation.</param>
        /// <param name="currentEvaluation">The current running evaluation.</param>
        protected virtual void OnIterationCompleted(BenchSharkResult currentIteration, BenchSharkResult currentEvaluation)
        {
            if (IterationCompleted != null)
            {
                IterationCompleted(this, new BenchSharkIterationEventArgs(currentIteration, currentEvaluation));
            }
        }
        #endregion
        #endregion

        #region Public Methods
        #region AddTask
        /// <summary>
        /// Adds a new task to the evaluation list.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="task">The task to evaluate.</param>
        public void AddTask(string name, Action task)
        {
            Tasks.Add(name, task);
        }
        #endregion
        #region ClearTasks
        /// <summary>
        /// Remove all the stored tasks.
        /// </summary>
        public void ClearTasks()
        {
            Tasks.Clear();
        }
        #endregion
        #region EvaluateStoredTasks
        /// <summary>
        /// Evaluate the performance of all the tasks previously stored.
        /// </summary>
        /// <param name="iterations">The number of iterations to evaluate the tasks.</param>
        /// <returns>The return value is an array containing the result of the evaluations.</returns>
        public IEnumerable<BenchSharkResult> EvaluateStoredTasks(uint iterations)
        {
            // Enumerate the tasks to evaluate
            foreach (var task in Tasks)
            {
                // Evaluate the task
                var result = EvaluateTask(task.Key, task.Value, iterations);
                // Raise the event for the completed evaluation
                OnEvaluationCompleted(result);
                // Return the result in a deferred manner
                yield return result;
            }
        }
        #endregion
        #region EvaluateTask
        /// <summary>
        /// Evaluate the performance of the task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="task">The task to evaluate.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        public BenchSharkResult EvaluateTask(string name, Action task)
        {
            return EvaluateTask(name, task, 1);
        }

        /// <summary>
        /// Evaluate the performance of the task.
        /// </summary>
        /// <param name="task">The task to evaluate.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        public BenchSharkResult EvaluateTask(Action task)
        {
            return EvaluateTask(null, task);
        }

        /// <summary>
        /// Evaluate the performance of the task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="task">The task to evaluate.</param>
        /// <param name="iterations">The number of iterations to evaluate the task.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        public BenchSharkResult EvaluateTask(string name, Action task, uint iterations)
        {
            // Execute the task without evaluating in order to run the jit compiler on it
            task();

            // Create the return value
            var evaluation = new BenchSharkResult
            {
                Name = name
            };

            // Perform the evaluation according the number of iterations
            for (var i = 0; i < iterations; i++)
            {
                // Check if the memory must be cleaned up
                if (i % CleanUpInterval == 0)
                {
                    // Clean the memory
                    CleanUpMemory();
                }

                // Perform the evaluation
                var iteration = InternalEvaluateTask(name, task);

                // Sum the result with the ones already saved
                evaluation += iteration;

                // Raise the event
                OnIterationCompleted(iteration, evaluation);
            }

            // Return the object
            return evaluation;
        }

        /// <summary>
        /// Evaluate the performance of the task.
        /// </summary>
        /// <param name="task">The task to evaluate.</param>
        /// <param name="iterations">The number of iterations to evaluate the task.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        public BenchSharkResult EvaluateTask(Action task, uint iterations)
        {
            return EvaluateTask(null, task, iterations);
        }

        #endregion
        #endregion
    }
}
