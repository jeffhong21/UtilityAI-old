namespace UtilityAI
{
    using UnityEngine;

    [System.Serializable]
    public class HasEnemies : ScorerBase
    {
        [SerializeField]
        public float score = 25;


        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;

            if (c.hostileEntities.Count == 0)
            {
                //Debug.Log("No hostileEntities");
                return 0f;
            }

            return this.score;
        }
    }
}
