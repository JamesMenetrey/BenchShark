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
using System.Reflection;
using Binarysharp.Benchmark.Helpers;
using Binarysharp.Benchmark.Internals;

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
        /// Determines whether the evaluation must store the result of each iteration (does not alter the exposed properties).
        /// If this value is set to <c>true</c>, the evaluation can be memory consuming, depending on the number of iterations.
        /// </summary>
        protected bool MustStoreIterations;

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

        /// <summary>
        /// Determines whether the object can run evaluations in debug mode.
        /// </summary>
        public bool EnableUnoptimizedEvaluations { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class <see cref="BenchShark"/>.
        /// </summary>
        /// <param name="mustStoreIterations">
        /// Determines whether the evaluation must store the result of each iteration (does not alter the exposed properties).
        /// If this value is set to <c>true</c>, the evaluation can be memory consuming, depending on the number of iterations.
        /// </param>
        /// <param name="cleanUpInterval">The number of interval of iteration to perform a memory clean up.</param>
        public BenchShark(bool mustStoreIterations = false, uint cleanUpInterval = 1)
        {
            // Initialize members
            Tasks = new Dictionary<string, Action>();
            CleanUpInterval = cleanUpInterval;
            MustStoreIterations = mustStoreIterations;
            EnableUnoptimizedEvaluations = false;
        }
        #endregion

        #region Protected Methods
        #region InternalEvaluateTask
        /// <summary>
        /// Evaluates the performance of the task.
        /// </summary>
        /// <param name="task">The task to evaluate.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        protected IterationResult InternalEvaluateTask(Action task)
        {
            // Initialize the stopwatch
            var watch = new Stopwatch();
            // Start the evaluation of the task
            watch.Start();
            // Run the task
            task();
            // Stop the evaluation
            watch.Stop();

            // Create en return the result
            return new IterationResult(watch.Elapsed, watch.ElapsedTicks);
        }
        #endregion
        #region OnEvaluationCompleted
        /// <summary>
        /// Raises the event <see cref="EvaluationCompleted"/>, stating that an evaluation was performed.
        /// </summary>
        /// <param name="taskEvaluated">The result of a task fully evaluated.</param>
        protected virtual void OnEvaluationCompleted(EvaluationResult taskEvaluated)
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
        protected virtual void OnIterationCompleted(IterationResult currentIteration, EvaluationResult currentEvaluation)
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
        #region EvaluateDecoratedTasks
        /// <summary>
        /// Evaluates the performance of the tasks decorated with the <see cref="BenchSharkTaskAttribute"/> attribute within the given object.
        /// </summary>
        /// <param name="obj">The object where the tasks are retrieved.</param>
        /// <param name="iterations">The number of iterations to evaluate the tasks.</param>
        /// <returns>The return value is an array containing the result of the evaluations.</returns>
        public EvaluationResultCollection EvaluateDecoratedTasks(object obj, uint iterations)
        {
            // Create the return value
            var collection = new EvaluationResultCollection();

            // Get the methods of the object that have the BenchShark attribute
            var tasks = obj.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                .Where(method => method.GetCustomAttributes(typeof(BenchSharkTaskAttribute), false).Length > 0);
            
            // Enumerate the tasks to evaluate
            foreach (var task in tasks)
            {
                // Get the name of the task
                var taskName = task.GetCustomAttribute<BenchSharkTaskAttribute>().Name;
                // Check that the task doesn't have parameter(s)
                if (task.GetParameters().Any())
                {
                    // Cannot invoke a task with parameter(s)
                    throw new Exception("Cannot run the task \"" + taskName + "\" because it has parameter(s).");
                }
                // Copy the foreach variable to a local variable
                var localTask = task;
                // Create the delegate to evaluate the task
                Action action = () => localTask.Invoke(obj, null);
                // Evaluate the task
                collection.AddEvaluationResult(EvaluateTask(taskName, action, iterations));
            }

            // Return the collection
            return collection;
        }

        /// <summary>
        /// Evaluates the performance of the tasks decorated with the <see cref="BenchSharkTaskAttribute"/> attribute within the given type.
        /// </summary>
        /// <typeparam name="T">The type where the methods are retrieved.</typeparam>
        /// <param name="iterations">The number of iterations to evaluate the tasks.</param>
        /// <returns>The return value is an array containing the result of the evaluations.</returns>
        /// <remarks>Calling this method creates a new instance of the given type.</remarks>
        public EvaluationResultCollection EvaluateDecoratedTasks<T>(uint iterations) where T : new()
        {
            // Create a new instance of the class
            var obj = new T();
            // Get the tasks by its object
            return EvaluateDecoratedTasks(obj, iterations);
        }
        #endregion
        #region EvaluateStoredTasks
        /// <summary>
        /// Evaluates the performance of all the tasks previously stored.
        /// </summary>
        /// <param name="iterations">The number of iterations to evaluate the tasks.</param>
        /// <returns>The return value is an array containing the result of the evaluations.</returns>
        public EvaluationResultCollection EvaluateStoredTasks(uint iterations)
        {
            // Create the return value
            var collection = new EvaluationResultCollection();

            // Enumerate the tasks to evaluate
            foreach (var task in Tasks)
            {
                // Evaluate the task
                collection.AddEvaluationResult(EvaluateTask(task.Key, task.Value, iterations));
            }

            // Return the collection
            return collection;
        }
        #endregion
        #region EvaluateTask
        /// <summary>
        /// Evaluate the performance of the task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="task">The task to evaluate.</param>
        /// <param name="iterations">The number of iterations to evaluate the task.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        public EvaluationResult EvaluateTask(string name, Action task, uint iterations)
        {
            // Check the process is optimized
            if (!EnableUnoptimizedEvaluations && !OptimizationHelper.IsOptimizedProcess)
            {
                throw new Exception("Cannot perform benchmark tests because the process is not running under an optimized state. " +
                    "Do not attach a debugger and compile the program under the Release mode in order to get the best performance. " +
                    "To remove this exception, set the property EnableUnoptimizedEvaluations to true.");
            }

            // Execute the task the first time to jit the function
            task();

            // Create the return value
            var evaluation = new EvaluationResult(name, MustStoreIterations);

            // Perform the evaluation according the number of iterations
            for (var i = 0; i < iterations; i++)
            {
                // Check if the memory must be cleaned up
                if (i % CleanUpInterval == 0)
                {
                    // Clean the memory
                    OptimizationHelper.OptimizeMemory();
                }

                // Perform the evaluation
                var iteration = InternalEvaluateTask(task);

                // Add the iteration to the evaluation result
                evaluation.AddIteration(iteration);

                // Raise the iteration event
                OnIterationCompleted(iteration, evaluation);
            }

            // Raise the evaluation event
            OnEvaluationCompleted(evaluation);

            // Return the object
            return evaluation;
        }

        /// <summary>
        /// Evaluate the performance of the task.
        /// </summary>
        /// <param name="task">The task to evaluate.</param>
        /// <param name="iterations">The number of iterations to evaluate the task.</param>
        /// <returns>The return value is the result of the evaluation.</returns>
        public EvaluationResult EvaluateTask(Action task, uint iterations)
        {
            return EvaluateTask(null, task, iterations);
        }

        #endregion
        #endregion
    }
}
