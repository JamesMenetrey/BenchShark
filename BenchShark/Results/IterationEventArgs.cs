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
    /// Represents an event argument when an iteration is completed.
    /// </summary>
    public class IterationEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// The result of the current iteration of the evaluation.
        /// </summary>
        public IterationResult CurrentIteration { get; protected set; }

        /// <summary>
        /// The result of the current running evaluation.
        /// </summary>
        public EvaluationResult CurrentEvaluation { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class <see cref="IterationEventArgs"/>.
        /// </summary>
        /// <param name="currentIteration">The current iteration of the evaluation.</param>
        /// <param name="currentEvaluation">The current running evaluation.</param>
        public IterationEventArgs(IterationResult currentIteration, EvaluationResult currentEvaluation)
        {
            CurrentIteration = currentIteration;
            CurrentEvaluation = currentEvaluation;
        }
        #endregion
    }
}
