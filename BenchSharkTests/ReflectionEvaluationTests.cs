/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System.Linq;
using Binarysharp.Benchmark;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenchSharkTests
{
    [TestClass]
    public class ReflectionEvaluationTests
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
        #endregion

        #region Tests
        /// <summary>
        /// Evaluates the tasks contained in this object with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateDecoratedTasks_Obj_OneIteration()
        {
            // Arrange
            var shark = new BenchShark(true) {EnableUnoptimizedEvaluations = true};
            SetupEnvironment();

            // Act
            var methods = shark.EvaluateDecoratedTasks(this, 1);

            // Assert
            Assert.AreEqual(3, methods.Evaluations.Count());
            Assert.AreNotEqual(methods.Evaluations.ElementAt(0), methods.Evaluations.ElementAt(1));
            Assert.AreEqual(2, FooRun); // one execution to jut the function, second to evaluate
            Assert.AreEqual(2, BarRun);
            Assert.AreEqual(2, StaticFoobarRun);
        }

        /// <summary>
        /// Evaluates the tasks contained in this type with one iteration.
        /// </summary>
        [TestMethod]
        public void EvaluateDecoratedTasks_Type_OneIteration()
        {
            // Arrange
            var shark = new BenchShark(true) {EnableUnoptimizedEvaluations = true};
            SetupEnvironment();

            // Act
            var methods = shark.EvaluateDecoratedTasks<ReflectionEvaluationTests>(1);

            // Assert
            Assert.AreEqual(3, methods.Evaluations.Count());
            Assert.AreNotEqual(methods.Evaluations.ElementAt(0), methods.Evaluations.ElementAt(1));
            Assert.AreEqual(2, FooRun);
            Assert.AreEqual(2, BarRun);
            Assert.AreEqual(2, StaticFoobarRun);
        }

        /// <summary>
        /// Evaluates the tasks contained in this object with 10 iterations.
        /// </summary>
        [TestMethod]
        public void EvaluateDecoratedTasks_Obj_TenIteration()
        {
            // Arrange
            var shark = new BenchShark(true) { EnableUnoptimizedEvaluations = true };
            SetupEnvironment();

            // Act
            var methods = shark.EvaluateDecoratedTasks(this, 10);

            // Assert
            Assert.AreEqual(3, methods.Evaluations.Count());
            Assert.AreNotEqual(methods.Evaluations.ElementAt(0), methods.Evaluations.ElementAt(1));
            Assert.AreEqual(11, FooRun); // one execution to jut the function, second to evaluate
            Assert.AreEqual(11, BarRun);
            Assert.AreEqual(11, StaticFoobarRun);
        }

        /// <summary>
        /// Evaluates the tasks contained in this type with 10 iterations.
        /// </summary>
        [TestMethod]
        public void EvaluateDecoratedTasks_Type_TenIteration()
        {
            // Arrange
            var shark = new BenchShark(true) { EnableUnoptimizedEvaluations = true };
            SetupEnvironment();

            // Act
            var methods = shark.EvaluateDecoratedTasks<ReflectionEvaluationTests>(10);

            // Assert
            Assert.AreEqual(3, methods.Evaluations.Count());
            Assert.AreNotEqual(methods.Evaluations.ElementAt(0), methods.Evaluations.ElementAt(1));
            Assert.AreEqual(11, FooRun);
            Assert.AreEqual(11, BarRun);
            Assert.AreEqual(11, StaticFoobarRun);
        }
        #endregion
    }
}
