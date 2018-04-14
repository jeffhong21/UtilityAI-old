namespace UtilityAI
{
    using UnityEngine;

    public class TestScorerB : ScorerBase
	{


        public override float Score(IAIContext context)
        {
            return Random.Range(0, 10);
        }
	}
}
