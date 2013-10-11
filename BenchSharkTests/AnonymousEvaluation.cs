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

            shark.IterationCompleted += (sender, args) =>
            {
                // Assert
                Assert.AreEqual(args.CurrentIteration.AverageElapsedTicks, args.CurrentIteration.TotalElapsedTicks);
                Assert.AreEqual(args.CurrentIteration.AverageExecutionTime, args.CurrentIteration.TotalExecutionTime);
                Assert.AreEqual(1, args.CurrentIteration.IterationsCount);
                Assert.AreEqual(null, args.CurrentIteration.Name);
                Assert.AreEqual(args.CurrentIteration.WorstExecutionTime, args.CurrentIteration.BestExecutionTime);
                Assert.AreEqual(args.CurrentIteration.WorstElapsedTicks, args.CurrentIteration.BestElapsedTicks);
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

            shark.IterationCompleted += (sender, args) =>
            {
                // Assert the current iteration
                Assert.AreEqual(args.CurrentIteration.AverageElapsedTicks, args.CurrentIteration.TotalElapsedTicks);
                Assert.AreEqual(args.CurrentIteration.AverageExecutionTime, args.CurrentIteration.TotalExecutionTime);
                Assert.AreEqual(1, args.CurrentIteration.IterationsCount);
                Assert.AreEqual(null, args.CurrentIteration.Name);
                Assert.IsTrue(args.CurrentIteration.WorstExecutionTime >= args.CurrentIteration.BestExecutionTime);
                Assert.IsTrue(args.CurrentIteration.WorstElapsedTicks >= args.CurrentIteration.BestElapsedTicks);

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
