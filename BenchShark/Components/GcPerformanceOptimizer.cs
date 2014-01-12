/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System.Runtime;

namespace Binarysharp.Benchmark.Components
{
    /// <summary>
    /// Class that creates a timeframe during which the performance of the current process is optimized by reducing the probability that the Garbage Collector runs.
    /// </summary>
    public class GcPerformanceOptimizer : IPerformanceOptimizer
    {
        #region Protected Fields
        /// <summary>
        /// The latency mode of the Garbage Collector before this object alterates it.
        /// </summary>
        protected GCLatencyMode OldGcLatencyMode;

        /// <summary>
        /// Defines whether the section is activated.
        /// </summary>
        protected bool IsSectionActivated;
        #endregion

        #region Destructor
        /// <summary>
        /// Frees resources and performs other cleanup operations before it is reclaimed by garbage collection. 
        /// </summary>
        ~GcPerformanceOptimizer()
        {
            // If the section was activated but never released
            if (IsSectionActivated)
            {
                // Leave the section
                LeaveOptimizedSection();
            }
        }
        #endregion

        #region Implementation of IPerformanceOptimizer
        /// <summary>
        /// Enters a section during which the performance of the current process is optimized.
        /// </summary>
        /// <remarks>
        /// Why use SustainedLowLatency instead of LowLatency: http://www.infoq.com/news/2012/03/Net-403.
        /// </remarks>
        public void EnterOptimizedSection()
        {
            // Flag the section as activated
            IsSectionActivated = true;

            // Store the current latency mode of the Garbage Collector
            OldGcLatencyMode = GCSettings.LatencyMode;

            // Set the latency to the lowest value
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }

        /// <summary>
        /// Leaves a section during which the performance of the current process is optimized.
        /// </summary>
        public void LeaveOptimizedSection()
        {
            // Flag the section as disabled
            IsSectionActivated = false;

            // Restores the old latency mode of the Garbage Collector.
            GCSettings.LatencyMode = OldGcLatencyMode;
        }
        #endregion
    }
}
