/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;
using System.Linq;
using Binarysharp.Benchmark;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenchSharkTests
{
    [TestClass]
    public class AnonymousEvaluation
    {
        public readonly Action TaskToEvaluate = () => string.Compare("Hello", "Hello", StringComparison.InvariantCulture);

        /// <summary>
        /// Evaluates an anonymous task with a simple evaluation.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_SimpleEvaluation()
        {
            // Arrange
            var shark = new BenchShark();

            // Act
            var result = shark.EvaluateTask(TaskToEvaluate);

            // Assert
            Assert.AreNotEqual(0, result.ElapsedTicks);
            Assert.AreNotEqual(TimeSpan.Zero, result.ExecutionTime);
            Assert.IsTrue(result.ElapsedTicks > result.ExecutionTime.TotalMilliseconds);
        }

        /// <summary>
        /// Evaluates an anonymous task with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_OneIteration()
        {
            // Arrange
            var shark = new BenchShark(1, true);

            // Act
            var result = shark.EvaluateTask(TaskToEvaluate, 1);

            // Assert
            Assert.AreEqual(1, result.IterationsCount);
            Assert.AreEqual(1, result.Iterations.Count());
            Assert.AreEqual(result.TotalElapsedTicks, result.AverageElapsedTicks);
            Assert.AreEqual(result.TotalExecutionTime, result.AverageExecutionTime);
            Assert.AreEqual(result.TotalElapsedTicks, result.BestElapsedTicks);
            Assert.AreEqual(result.TotalElapsedTicks, result.WorstElapsedTicks);
            Assert.AreEqual(result.TotalExecutionTime, result.BestExecutionTime);
            Assert.AreEqual(result.TotalExecutionTime, result.WorstExecutionTime);
        }


        /// <summary>
        /// Evaluates an anonymous task with one iteration using the iteration event.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_OneIteration_WithIterationEvent()
        {
            // Arrange
            var shark = new BenchShark();

            shark.IterationCompleted += (sender, args) =>
            {
                // Assert
                Assert.AreNotEqual(0, args.CurrentIteration.ElapsedTicks);
                Assert.AreNotEqual(TimeSpan.Zero, args.CurrentIteration.ExecutionTime);
                Assert.IsTrue(args.CurrentIteration.ElapsedTicks > args.CurrentIteration.ExecutionTime.TotalMilliseconds);

                Assert.AreEqual(1, args.CurrentEvaluation.IterationsCount);
                Assert.AreEqual(1, args.CurrentEvaluation.Iterations.Count());
                Assert.AreEqual(args.CurrentEvaluation.TotalElapsedTicks, args.CurrentEvaluation.AverageElapsedTicks);
                Assert.AreEqual(args.CurrentEvaluation.TotalExecutionTime, args.CurrentEvaluation.AverageExecutionTime);
                Assert.AreEqual(args.CurrentEvaluation.TotalElapsedTicks, args.CurrentEvaluation.BestElapsedTicks);
                Assert.AreEqual(args.CurrentEvaluation.TotalElapsedTicks, args.CurrentEvaluation.WorstElapsedTicks);
                Assert.AreEqual(args.CurrentEvaluation.TotalExecutionTime, args.CurrentEvaluation.BestExecutionTime);
                Assert.AreEqual(args.CurrentEvaluation.TotalExecutionTime, args.CurrentEvaluation.WorstExecutionTime);

                Assert.AreEqual(args.CurrentIteration.ElapsedTicks, args.CurrentEvaluation.TotalElapsedTicks);
                Assert.AreEqual(args.CurrentIteration.ExecutionTime, args.CurrentEvaluation.TotalExecutionTime);
            };

            // Act
            shark.EvaluateTask(TaskToEvaluate);
        }

        /// <summary>
        /// Evaluates an anonymous task with 10 iterations.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TenIterations()
        {
            // Arrange
            var shark = new BenchShark(1, true);

            // Act
            var result = shark.EvaluateTask(TaskToEvaluate, 10);

            // Assert
            Assert.AreEqual(10, result.IterationsCount);
            Assert.AreEqual(10, result.Iterations.Count());
            Assert.IsTrue(result.AverageElapsedTicks < result.TotalElapsedTicks);
            Assert.IsTrue(result.AverageExecutionTime < result.TotalExecutionTime);
            Assert.AreEqual(null, result.Name);
            Assert.IsTrue(result.WorstExecutionTime > result.BestExecutionTime);
            Assert.IsTrue(result.WorstElapsedTicks > result.BestElapsedTicks);
        }

        /// <summary>
        /// Evaluates an anonymous task with 10 iterations using the iteration event.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TenIterations_WithIterationEvent()
        {
            // Arrange
            var shark = new BenchShark();
            var counter = 0;

            shark.IterationCompleted += (sender, args) =>
            {
                // Assert the current iteration
                Assert.IsTrue(args.CurrentIteration.ElapsedTicks > args.CurrentIteration.ExecutionTime.TotalMilliseconds);

                // Assert the current evaluation
                Assert.AreEqual(++counter, args.CurrentEvaluation.IterationsCount);
                Assert.AreEqual(null, args.CurrentEvaluation.Name);
                // The following is true when more than one iteration is performed
                if (args.CurrentEvaluation.IterationsCount > 1)
                {
                    Assert.IsTrue(args.CurrentEvaluation.AverageElapsedTicks < args.CurrentEvaluation.TotalElapsedTicks);
                    Assert.IsTrue(args.CurrentEvaluation.AverageExecutionTime < args.CurrentEvaluation.TotalExecutionTime);
                    Assert.IsTrue(args.CurrentEvaluation.WorstExecutionTime > args.CurrentEvaluation.BestExecutionTime);
                    Assert.IsTrue(args.CurrentEvaluation.WorstElapsedTicks > args.CurrentEvaluation.BestElapsedTicks);
                }
            };

            // Act
            var result = shark.EvaluateTask(TaskToEvaluate, 10);

            // Assert
            Assert.AreEqual(10, result.IterationsCount);
        }
    }
}
