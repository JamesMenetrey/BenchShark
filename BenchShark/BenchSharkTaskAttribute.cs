using System;

namespace Binarysharp.Benchmark
{
    /// <summary>
    /// Represents an attribute that BenchMark can evaluate.
    /// </summary>
    public class BenchSharkTaskAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// The name of the task to evaluate.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
