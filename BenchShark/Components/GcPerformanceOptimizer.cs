using System.Runtime;

namespace Binarysharp.Benchmark.Components
{
    /// <summary>
    /// Class that creates a timeframe during which the performance of the current process is optimized by reducing the probability that the Garbage Collector runs.
    /// </summary>
    public class GcPerformanceOptimizer : IPerformanceOptimizer
    {
        /// <summary>
        /// The latency mode of the Garbage Collector before this object alterates it.
        /// </summary>
        protected GCLatencyMode OldGcLatencyMode;

        /// <summary>
        /// Enters a section during which the performance of the current process is optimized.
        /// </summary>
        /// <remarks>
        /// Why use SustainedLowLatency instead of LowLatency: http://www.infoq.com/news/2012/03/Net-403.
        /// </remarks>
        public void EnterOptimizedSection()
        {
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
            // Restores the old latency mode of the Garbage Collector.
            GCSettings.LatencyMode = OldGcLatencyMode;
        }
    }
}
