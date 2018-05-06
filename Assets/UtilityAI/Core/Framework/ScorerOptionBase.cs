namespace UtilityAI
{
    using UnityEngine;
    using System;
    /// <summary>
    /// Option scorer base.  This generates a score for ActionWithOptions and generates a score for an indivual option.
    /// </summary>
    [Serializable]
    public abstract class ScorerOptionBase<TOption> : IOptionScorer<TOption>
    {

        public abstract float Score(IAIContext context, TOption data);

    }



}