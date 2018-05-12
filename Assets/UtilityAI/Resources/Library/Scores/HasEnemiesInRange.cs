namespace UtilityAI
{
    using UnityEngine;

    [System.Serializable]
    public class HasEnemiesInRange : ScorerBase
    {
        [SerializeField]
        public float score = 25;
        [SerializeField]
        public float range = 10f;  //  entites within this range


        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;

            //var _score = score;
            //_score += UnityEngine.Random.Range(-score / 10 , score / 10);


            var hostileEntities = c.hostileEntities;

            var count = hostileEntities.Count;

            for (int i = 0; i < count; i++)
            {
                var enemy = hostileEntities[i];
                var sqrDist = (enemy.position - c.entity.transform.position).sqrMagnitude;

                //  If enemy is within range.
                if (sqrDist <= range * range)
                    return score;
            }

            return 0f;

        }
    }
}