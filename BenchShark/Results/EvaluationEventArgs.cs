/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System;

namespace Binarysharp.Benchmark.Results
{
    /// <summary>
    /// Represents an event argument when a evaluation is completed.
    /// </summary>
    public class EvaluationEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// The result of a task fully evaluated.
        /// </summary>
        public EvaluationResult TaskEvaluated { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class <see cref="EvaluationEventArgs"/>.
        /// </summary>
        /// <param name="taskEvaluated">The result of a task fully evaluated.</param>
        internal EvaluationEventArgs(EvaluationResult taskEvaluated)
        {
            TaskEvaluated = taskEvaluated;
        }
        #endregion
    }
}
