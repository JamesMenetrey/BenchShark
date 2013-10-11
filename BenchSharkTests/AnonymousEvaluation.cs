using System;
using Binarysharp.Benchmark;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenchSharkTests
{
    [TestClass]
    public class AnonymousEvaluation
    {
        public readonly Action TaskToEvaluate = () => string.Compare("Hello", "Hello", StringComparison.InvariantCulture);

        /// <summary>
        /// Evaluates an anonymous task with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateNamedTask_OneIteration()
        {
            // Arrange
            var shark = new BenchShark();
            const string name = "Foo";

            // Act
            var result = shark.EvaluateTask(name, TaskToEvaluate);

            // Assert
            Assert.AreEqual(name, result.Name);
        }

        /// <summary>
        /// Evaluates an anonymous task with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_OneIteration()
        {
            // Arrange
            var shark = new BenchShark();

            // Act
            var result = shark.EvaluateTask(TaskToEvaluate);

            // Assert
            Assert.AreEqual(result.AverageElapsedTicks, result.TotalElapsedTicks);
            Assert.AreEqual(result.AverageExecutionTime, result.TotalExecutionTime);
            Assert.AreEqual(1, result.IterationsCount);
            Assert.AreEqual(null, result.Name);
            Assert.IsTrue(result.WorstExecutionTime >= result.BestExecutionTime);
            Assert.IsTrue(result.WorstElapsedTicks >= result.BestElapsedTicks);
        }


        /// <summary>
        /// Evaluates an anonymous task with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_OneIteration_WithEvent()
        {
            // Arrange
            var shark = new BenchShark();

            // Act
            shark.TaskEvaluated += (sender, args) =>
            {
                // Assert
                Assert.AreEqual(args.Result.AverageElapsedTicks, args.Result.TotalElapsedTicks);
                Assert.AreEqual(args.Result.AverageExecutionTime, args.Result.TotalExecutionTime);
                Assert.AreEqual(1, args.Result.IterationsCount);
                Assert.AreEqual(null, args.Result.Name);
                Assert.IsTrue(args.Result.WorstExecutionTime >= args.Result.BestExecutionTime);
                Assert.IsTrue(args.Result.WorstElapsedTicks >= args.Result.BestElapsedTicks);
            };
            var result = shark.EvaluateTask(TaskToEvaluate);
        }

        /// <summary>
        /// Evaluates an anonymous task with 10 iterations.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TenIterations()
        {
            // Arrange
            var shark = new BenchShark();

            // Act
            var result = shark.EvaluateTask(TaskToEvaluate, 10);

            // Assert
            Assert.IsTrue(result.AverageElapsedTicks < result.TotalElapsedTicks);
            Assert.IsTrue(result.AverageExecutionTime < result.TotalExecutionTime);
            Assert.AreEqual(10, result.IterationsCount);
            Assert.AreEqual(null, result.Name);
            Assert.IsTrue(result.WorstExecutionTime > result.BestExecutionTime);
            Assert.IsTrue(result.WorstElapsedTicks > result.BestElapsedTicks);
        }

        /// <summary>
        /// Evaluates an anonymous task with 10 iterations.
        /// </summary>
        [TestMethod]
        public void EvaluateTask_TenIterations_WithEvent()
        {
            // Arrange
            var shark = new BenchShark();
            var counter = 0;

            // Act
            shark.TaskEvaluated += (sender, args) =>
            {
                // Assert Result
                Assert.AreEqual(args.Result.AverageElapsedTicks, args.Result.TotalElapsedTicks);
                Assert.AreEqual(args.Result.AverageExecutionTime, args.Result.TotalExecutionTime);
                Assert.AreEqual(1, args.Result.IterationsCount);
                Assert.AreEqual(null, args.Result.Name);
                Assert.IsTrue(args.Result.WorstExecutionTime >= args.Result.BestExecutionTime);
                Assert.IsTrue(args.Result.WorstElapsedTicks >= args.Result.BestElapsedTicks);

                // Assert Total Result
                Assert.AreEqual(++counter, args.TotalResults.IterationsCount);
                Assert.AreEqual(args.Result.AverageElapsedTicks, args.Result.TotalElapsedTicks);
                Assert.AreEqual(args.Result.AverageExecutionTime, args.Result.TotalExecutionTime);
                Assert.AreEqual(1, args.Result.IterationsCount);
                Assert.AreEqual(null, args.Result.Name);
                Assert.IsTrue(args.Result.WorstExecutionTime >= args.Result.BestExecutionTime);
                Assert.IsTrue(args.Result.WorstElapsedTicks >= args.Result.BestElapsedTicks);
            };
            shark.EvaluateTask(TaskToEvaluate, 10);
        }
    }
}
