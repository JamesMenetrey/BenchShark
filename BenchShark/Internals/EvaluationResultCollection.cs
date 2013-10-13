/*
 * BenchShark library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2013 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/
using System.Collections.Generic;
using System.Linq;

namespace Binarysharp.Benchmark.Internals
{
    /// <summary>
    /// Provides a strongly typed collection of <see cref="EvaluationResult"/> objects.
    /// </summary>
    class EvaluationResultCollection
    {
        #region Properties
        /// <summary>
        /// The results of the completed evaluations ordered by the fastest speed of execution.
        /// </summary>
        public IEnumerable<EvaluationResult> FastestEvaluations
        {
            get
            {
                return Evaluations.OrderBy(evaluation => evaluation.TotalElapsedTicks);
            }
        }

        /// <summary>
        /// The results of the completed evaluations.
        /// </summary>
        public IEnumerable<EvaluationResult> Evaluations { get; protected set; }

        /// <summary>
        /// The results of the completed evaluations ordered by the slowest speed of execution.
        /// </summary>
        public IEnumerable<EvaluationResult> SlowestEvaluations
        {
            get
            {
                return Evaluations.OrderByDescending(evaluation => evaluation.TotalElapsedTicks);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationResultCollection"/> class.
        /// </summary>
        internal EvaluationResultCollection(IEnumerable<EvaluationResult> evaluations)
        {
            Evaluations = evaluations;
        }
        #endregion
    }
}
