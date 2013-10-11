/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/

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
