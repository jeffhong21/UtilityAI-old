namespace UtilityAI
{
    using UnityEngine;
    using System;


    public abstract class ScorerBase : IScorer
    {
        public int score;
        public abstract float Score(IAIContext context);

    }
}