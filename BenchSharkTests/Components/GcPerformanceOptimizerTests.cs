/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;
using System.Runtime;
using Binarysharp.Benchmark.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenchSharkTests.Components
{
    [TestClass]
    public class GcPerformanceOptimizerTests
    {
        /// <summary>
        /// Checks that the latency modes are correctly set.
        /// </summary>
        [TestMethod]
        public void GcLatencyMode_TestValues()
        {
            // Arrange
            var p = new GcPerformanceOptimizer();

            // Act
            var beforeMode = GCSettings.LatencyMode;

            p.EnterOptimizedSection();
            var whileMode = GCSettings.LatencyMode;

            p.LeaveOptimizedSection();
            var afterMode = GCSettings.LatencyMode;

            // Assert
            Assert.AreEqual(afterMode, beforeMode);
            Assert.AreEqual(GCLatencyMode.SustainedLowLatency, whileMode);
        }

        /// <summary>
        /// Checks that the latency mode is correctly restored when the object is collected and the Leave method wasn't called.
        /// </summary>
        [TestMethod]
        public void GcLatencyMode_RestoredWithMissingLeave()
        {
            // Arrange
            var beforeMode = GCSettings.LatencyMode;

            // Act
            {
                new GcPerformanceOptimizer().EnterOptimizedSection();

            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.AreEqual(beforeMode, GCSettings.LatencyMode);
        }
    }
}
