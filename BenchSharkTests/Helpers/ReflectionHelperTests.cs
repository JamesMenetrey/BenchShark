using System;
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
        protected static int FooRun;

        /// <summary>
        /// The number of times the method <see cref="Bar"/> ran.
        /// </summary>
        protected static int BarRun;

        /// <summary>
        /// The number of times the static method <see cref="Foobar"/> ran.
        /// </summary>
        protected static int StaticFoobarRun;

        /// <summary>
        /// Set up the test environment.
        /// </summary>
        protected void SetupEnvironment()
        {
            FooRun = 0;
            BarRun = 0;
            StaticFoobarRun = 0;
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

        /// <summary>
        /// A benchmarkable static method.
        /// </summary>
        [BenchSharkTask("Foobar")]
        protected static void Foobar()
        {
            StaticFoobarRun++;
        }

        public class NestedHelperTests
        {
            /// <summary>
            /// A benchmarkable method with using a parameter.
            /// </summary>
            [BenchSharkTask("MethodWithParam")]
            protected int MethodWithParam(int param)
            {
                return param;
            }
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
            Assert.AreEqual(3, delegates.Count());
            Assert.AreNotEqual(delegates.ElementAt(0), delegates.ElementAt(1));
            Assert.AreEqual(1, FooRun);
            Assert.AreEqual(1, BarRun);
            Assert.AreEqual(1, StaticFoobarRun);
        }

        /// <summary>
        /// Gets the tasks by this test class.
        /// </summary>
        [TestMethod]
        public void GetTasksByType_This()
        {
            // Arrange/Act
            try
            {
                ReflectionHelper.GetTasksByType<NestedHelperTests>().ToArray();
                // Assert
                Assert.Fail("The library cannot handle function with parameter(s).");
            }
            catch (Exception ex)
            {
                if (ex is AssertFailedException)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
