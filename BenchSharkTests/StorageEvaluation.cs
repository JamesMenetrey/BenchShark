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
    public class StorageEvaluation
    {
        public readonly Action TaskToEvaluate1 = () => string.Compare("Hello", "Hello", StringComparison.InvariantCulture);
        public readonly Action TaskToEvaluate2 = () => "Hello".Equals("Hello");

        /// <summary>
        /// Evaluates a stored task with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_OneTask_OneIteration()
        {
            // Arrange
            var shark = new BenchShark();
            const string name = "Foo";

            // Act
            shark.AddTask(name, TaskToEvaluate1);
            var allResults = shark.EvaluateStoredTasks(1);
            var result = allResults.Evaluations.First();

            // Assert collection
            Assert.AreEqual(1, allResults.Evaluations.Count());
            CollectionAssert.AreEqual(allResults.FastestEvaluations.ToArray(), allResults.SlowestEvaluations.ToArray());

            // Assert evaluation
            Assert.AreEqual(result.AverageElapsedTicks, result.TotalElapsedTicks);
            Assert.AreEqual(result.AverageExecutionTime, result.TotalExecutionTime);
            Assert.AreEqual(1, result.IterationsCount);
            Assert.AreEqual(name, result.Name);
            Assert.IsTrue(result.WorstExecutionTime == result.BestExecutionTime);
            Assert.IsTrue(result.WorstElapsedTicks == result.BestElapsedTicks);
        }

        /// <summary>
        /// Evaluates a stored task with ten iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_OneTask_TenIteration()
        {
            // Arrange
            var shark = new BenchShark();
            const string name = "Foo";

            // Act
            shark.AddTask(name, TaskToEvaluate1);
            var allResults = shark.EvaluateStoredTasks(10);
            var result = allResults.Evaluations.First();

            // Assert collection
            Assert.AreEqual(1, allResults.Evaluations.Count());
            CollectionAssert.AreEqual(allResults.FastestEvaluations.ToArray(), allResults.SlowestEvaluations.ToArray());

            // Assert evaluation
            Assert.IsTrue(result.AverageElapsedTicks < result.TotalElapsedTicks);
            Assert.IsTrue(result.AverageExecutionTime < result.TotalExecutionTime);
            Assert.AreEqual(10, result.IterationsCount);
            Assert.AreEqual(name, result.Name);
            Assert.IsTrue(result.WorstExecutionTime > result.BestExecutionTime);
            Assert.IsTrue(result.WorstElapsedTicks > result.BestElapsedTicks);
        }

        /// <summary>
        /// Evaluates 2 stored task with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TwoTasks_OneIteration()
        {
            // Arrange
            var shark = new BenchShark();
            const string name1 = "Foo";
            const string name2 = "Bar";

            // Act
            shark.AddTask(name1, TaskToEvaluate1);
            shark.AddTask(name2, TaskToEvaluate2);
            var allResults = shark.EvaluateStoredTasks(1);

            // Assert collection
            Assert.AreEqual(2, allResults.Evaluations.Count());
            Assert.AreNotEqual(allResults.FastestEvaluations, allResults.SlowestEvaluations);

            // Assert evaluation
            Assert.AreEqual(name1, allResults.Evaluations.ElementAt(0).Name);
            Assert.AreEqual(name2, allResults.Evaluations.ElementAt(1).Name);
            foreach (var result in allResults.Evaluations)
            {
                Assert.AreEqual(result.AverageElapsedTicks, result.TotalElapsedTicks);
                Assert.AreEqual(result.AverageExecutionTime, result.TotalExecutionTime);
                Assert.AreEqual(1, result.IterationsCount);
                Assert.IsTrue(result.WorstExecutionTime == result.BestExecutionTime);
                Assert.IsTrue(result.WorstElapsedTicks == result.BestElapsedTicks);
            }
        }

        /// <summary>
        /// Evaluates 2 stored task with 10 iterations.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TwoTasks_TenIteration()
        {
            // Arrange
            var shark = new BenchShark();
            const string name1 = "Foo";
            const string name2 = "Bar";

            // Act
            shark.AddTask(name1, TaskToEvaluate1);
            shark.AddTask(name2, TaskToEvaluate2);
            var allResults = shark.EvaluateStoredTasks(10);

            // Assert collection
            Assert.AreEqual(2, allResults.Evaluations.Count());
            Assert.AreNotEqual(allResults.FastestEvaluations, allResults.SlowestEvaluations);

            // Assert evaluation
            Assert.AreEqual(name1, allResults.Evaluations.ElementAt(0).Name);
            Assert.AreEqual(name2, allResults.Evaluations.ElementAt(1).Name);
            foreach (var result in allResults.Evaluations)
            {
                Assert.IsTrue(result.AverageElapsedTicks < result.TotalElapsedTicks);
                Assert.IsTrue(result.AverageExecutionTime < result.TotalExecutionTime);
                Assert.AreEqual(10, result.IterationsCount);
                Assert.IsTrue(result.WorstExecutionTime > result.BestExecutionTime);
                Assert.IsTrue(result.WorstElapsedTicks > result.BestElapsedTicks);
            }
        }

        /// <summary>
        /// Evaluates 2 stored task with 10 iterations using the evaluation event.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TwoTasks_TenIteration_WithEvaluationEvent()
        {
            // Arrange
            var shark = new BenchShark();
            var names = new[] { "Foo", "Bar" };
            var counter = 0;

            shark.EvaluationCompleted += (sender, args) =>
            {
                Assert.AreEqual(names[counter], args.TaskEvaluated.Name);
                Assert.IsTrue(args.TaskEvaluated.AverageElapsedTicks < args.TaskEvaluated.TotalElapsedTicks);
                Assert.IsTrue(args.TaskEvaluated.AverageExecutionTime < args.TaskEvaluated.TotalExecutionTime);
                Assert.AreEqual(10, args.TaskEvaluated.IterationsCount);
                Assert.IsTrue(args.TaskEvaluated.WorstExecutionTime > args.TaskEvaluated.BestExecutionTime);
                Assert.IsTrue(args.TaskEvaluated.WorstElapsedTicks > args.TaskEvaluated.BestElapsedTicks);
                counter++;
            };

            // Act
            shark.AddTask(names[0], TaskToEvaluate1);
            shark.AddTask(names[1], TaskToEvaluate2);
            shark.EvaluateStoredTasks(10);

            // Assert
            Assert.AreEqual(2, counter);
        }
    }
}
