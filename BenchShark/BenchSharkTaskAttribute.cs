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
    /// This attribute can decorate instance and static methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BenchSharkTaskAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// The name of the task to evaluate.
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BenchSharkTaskAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        public BenchSharkTaskAttribute(string name)
        {
            Name = name;
        }
        #endregion
    }
}
