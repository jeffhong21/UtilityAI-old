namespace UtilityAI
{
    using UnityEngine;

    public class TestScorerA : ScorerBase
    {


        public override float Score(IAIContext context)
        {
            return Random.Range(0, 10);
        }
    }
}
