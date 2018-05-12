namespace UtilityAI
{
    using UnityEngine;


    [System.Serializable]
    public class TestScorerA : ScorerBase
    {
        
        public override float Score(IAIContext context)
        {
            return Random.Range(0, 10);
        }
    }
}
