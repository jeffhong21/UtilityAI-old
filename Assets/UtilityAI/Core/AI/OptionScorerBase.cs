namespace UtilityAI
{
    using UnityEngine;

    /// <summary>
    /// Option scorer base.  This generates a score for ActionWithOptions and generates a score for an indivual option.
    /// </summary>
    public abstract class OptionScorerBase<TOption> : IOptionScorer<TOption>
    {

        public abstract float Score(IContext context, TOption data);

    }



}