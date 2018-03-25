//namespace UtilityAI
//{
//    using UnityEngine;

//    public class HasEnemiesInRange : ScorerBase
//    {
//        [SerializeField]
//        public float score = 50;
//        [SerializeField]
//        public float range = 3f;


//        public override float Score(IContext context)
//        {
//            var c = (AIContext)context;

//            //var _score = score;
//            //_score += UnityEngine.Random.Range(-score / 10 , score / 10);


//            var enemies = c.enemies;

//            var count = enemies.Count;

//            for (int i = 0; i < count; i++)
//            {
//                var enemy = enemies[i];
//                var sqrDist = (enemy.transform.position - c.entity.transform.position).sqrMagnitude;

//                //  If enemy is within range.
//                if (sqrDist <= range * range)
//                    return score;
//            }

//            return 0f;

//        }
//    }
//}