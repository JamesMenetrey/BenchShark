using System.Linq;
using Binarysharp.Benchmark;
using Binarysharp.Benchmark.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenchSharkTests.Helpers
{
    [TestClass]
    public class ReflectionHelperTests
    {
        #region Environment
        /// <summary>
        /// The number of times the method <see cref="Foo"/> ran.
        /// </summary>
        protected int FooRun = 0;
        /// <summary>
        /// The number of times the method <see cref="Bar"/> ran.
        /// </summary>
        protected int BarRun = 0;

        /// <summary>
        /// Set up the test environment.
        /// </summary>
        protected void SetupEnvironment()
        {
            FooRun = 0;
            BarRun = 0;
        }

        /// <summary>
        /// A benchmarkable method.
        /// </summary>
        [BenchSharkTask("Foo")]
        protected void Foo()
        {
            FooRun++;
        }

        /// <summary>
        /// A benchmarkable method.
        /// </summary>
        [BenchSharkTask("Bar")]
        protected void Bar()
        {
            BarRun++;
        }
        #endregion

        #region Tests
        /// <summary>
        /// Gets the tasks by a reference of an object.
        /// </summary>
        [TestMethod]
        public void GetTasksByObject_This()
        {
            // Arrange
            SetupEnvironment();

            // Act
            var delegates = ReflectionHelper.GetTasksByObject(this).ToArray();

            foreach (var action in delegates)
            {
                action.Invoke();
            }

            // Assert
            Assert.AreEqual(2, delegates.Count());
            Assert.AreNotEqual(delegates.ElementAt(0), delegates.ElementAt(1));
            Assert.AreEqual(1, FooRun);
            Assert.AreEqual(1, BarRun);
        }
        #endregion
    }
}
