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
            var result = shark.EvaluateTask(TaskToEvaluate, 1);

            // Assert
            Assert.AreNotEqual(0, result.TotalElapsedTicks);
            Assert.AreNotEqual(TimeSpan.Zero, result.TotalExecutionTime);
            Assert.IsTrue(result.TotalElapsedTicks > result.TotalExecutionTime.TotalMilliseconds);
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
            var shark = new BenchShark(1, true);
            var passed = false;

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
                passed = true;
            };

            // Act
            shark.EvaluateTask(TaskToEvaluate, 1);

            // Assert
            Assert.IsTrue(passed);
        }

        /// <summary>
        /// Evaluates an anonymous task with one iteration using the evaluation event.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_OneIteration_WithEvaluationEvent()
        {
            // Arrange
            var shark = new BenchShark(1, true);
            var passed = false;

            shark.EvaluationCompleted += (sender, args) =>
            {
                // Assert
                Assert.AreNotEqual(0, args.TaskEvaluated.TotalElapsedTicks);
                Assert.AreNotEqual(TimeSpan.Zero, args.TaskEvaluated.TotalExecutionTime);
                Assert.IsTrue(args.TaskEvaluated.TotalElapsedTicks > args.TaskEvaluated.TotalExecutionTime.TotalMilliseconds);

                Assert.AreEqual(1, args.TaskEvaluated.IterationsCount);
                Assert.AreEqual(1, args.TaskEvaluated.Iterations.Count());
                Assert.AreEqual(args.TaskEvaluated.TotalElapsedTicks, args.TaskEvaluated.AverageElapsedTicks);
                Assert.AreEqual(args.TaskEvaluated.TotalExecutionTime, args.TaskEvaluated.AverageExecutionTime);
                Assert.AreEqual(args.TaskEvaluated.TotalElapsedTicks, args.TaskEvaluated.BestElapsedTicks);
                Assert.AreEqual(args.TaskEvaluated.TotalElapsedTicks, args.TaskEvaluated.WorstElapsedTicks);
                Assert.AreEqual(args.TaskEvaluated.TotalExecutionTime, args.TaskEvaluated.BestExecutionTime);
                Assert.AreEqual(args.TaskEvaluated.TotalExecutionTime, args.TaskEvaluated.WorstExecutionTime);

                Assert.AreEqual(args.TaskEvaluated.TotalElapsedTicks, args.TaskEvaluated.TotalElapsedTicks);
                Assert.AreEqual(args.TaskEvaluated.TotalExecutionTime, args.TaskEvaluated.TotalExecutionTime);
                passed = true;
            };

            // Act
            shark.EvaluateTask(TaskToEvaluate, 1);

            // Assert
            Assert.IsTrue(passed);
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
            var passed = false;

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
                    passed = true;
                }
            };

            // Act
            var result = shark.EvaluateTask(TaskToEvaluate, 10);

            // Assert
            Assert.AreEqual(10, result.IterationsCount);
            Assert.IsTrue(passed);
        }

        /// <summary>
        /// Evaluates an anonymous task with 10 iterations using the iteration event.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TenIterations_WithEvaluationEvent()
        {
            // Arrange
            var shark = new BenchShark();
            var counter = 0;
            var passed = false;

            shark.EvaluationCompleted += (sender, args) =>
            {
                counter++;
                Assert.IsTrue(args.TaskEvaluated.TotalElapsedTicks > args.TaskEvaluated.TotalExecutionTime.TotalMilliseconds);
                Assert.AreEqual(null, args.TaskEvaluated.Name);
                Assert.IsTrue(args.TaskEvaluated.AverageElapsedTicks < args.TaskEvaluated.TotalElapsedTicks);
                Assert.IsTrue(args.TaskEvaluated.AverageExecutionTime < args.TaskEvaluated.TotalExecutionTime);
                Assert.IsTrue(args.TaskEvaluated.WorstExecutionTime > args.TaskEvaluated.BestExecutionTime);
                Assert.IsTrue(args.TaskEvaluated.WorstElapsedTicks > args.TaskEvaluated.BestElapsedTicks);
                passed = true;
            };

            // Act
            shark.EvaluateTask(TaskToEvaluate, 10);

            // Assert
            Assert.AreEqual(1, counter);
            Assert.IsTrue(passed);
        }
    }
}
