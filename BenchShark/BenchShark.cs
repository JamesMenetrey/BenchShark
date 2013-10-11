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
        /// Event fired when an evaluation of a task is performed.
        /// </summary>
        public event EventHandler<BenchSharkEventArgs> TaskEvaluated;
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
        #region OnTaskEvaluated
        /// <summary>
        /// Raises the event <see cref="TaskEvaluated"/>.
        /// </summary>
        /// <param name="e">The event argument to pass to the event.</param>
        protected virtual void OnTaskEvaluated(BenchSharkEventArgs e)
        {
            if (TaskEvaluated != null)
            {
                TaskEvaluated(this, e);
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
        #region EvaluateAllTasks
        /// <summary>
        /// Evaluate the performance of all the tasks previously stored.
        /// </summary>
        /// <param name="iterations">The number of iterations to evaluate the tasks.</param>
        /// <returns>The return value is an array containing the result of the evaluations.</returns>
        public IEnumerable<BenchSharkResult> EvaluateStoredTasks(uint iterations)
        {
            return Tasks.Select(task => EvaluateTask(task.Key, task.Value, iterations));
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
            var allResults = new BenchSharkResult
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
                var result = InternalEvaluateTask(name, task);

                // Set the number of iterations
                allResults.IterationsCount++;

                // Add the execution time/tick
                allResults.TotalElapsedTicks += result.TotalElapsedTicks;
                allResults.TotalExecutionTime += result.TotalExecutionTime;

                // Check the best case
                allResults.BestElapsedTicks = result.BestElapsedTicks < allResults.BestElapsedTicks
                    ? result.BestElapsedTicks
                    : allResults.BestElapsedTicks;
                allResults.BestExecutionTime = result.BestExecutionTime < allResults.BestExecutionTime
                    ? result.BestExecutionTime
                    : allResults.BestExecutionTime;

                // Check the worst case
                allResults.WorstElapsedTicks = result.WorstElapsedTicks > allResults.WorstElapsedTicks
                    ? result.WorstElapsedTicks
                    : allResults.WorstElapsedTicks;
                allResults.WorstExecutionTime = result.WorstExecutionTime > allResults.WorstExecutionTime
                    ? result.WorstExecutionTime
                    : allResults.WorstExecutionTime;

                // Raise the event
                OnTaskEvaluated(new BenchSharkEventArgs(result, allResults));
            }

            // Return the object
            return allResults;
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
