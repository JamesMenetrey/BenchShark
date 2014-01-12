namespace Binarysharp.Benchmark.Components
{
    /// <summary>
    /// Defines a set of methods that creates a timeframe during which the performance of the current process is optimized.
    /// </summary>
    public interface IPerformanceOptimizer
    {
        /// <summary>
        /// Enters a section during which the performance of the current process is optimized.
        /// </summary>
        void EnterOptimizedSection();

        /// <summary>
        /// Leaves a section during which the performance of the current process is optimized.
        /// </summary>
        void LeaveOptimizedSection();
    }
}
