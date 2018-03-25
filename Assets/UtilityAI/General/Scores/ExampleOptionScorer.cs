//namespace UtilityAI
//{
//    using UnityEngine;


//    /// <summary>
//    /// Distance from Player
//    /// </summary>
//    public sealed class ExampleOptionScorer : OptionScorerBase<Vector3>
//    {
//        public float score = 25;
//        public float desiredRange = 14f;


//        public override float Score(IContext context, Vector3 position)
//        {
//            /* Put logic here */

//            var c = (AIContext)context;

//            var enemies = c.enemies;
//            var count = enemies.Count;
//            if (count == 0)
//            {
//                return 0f;
//            }

//            var nearest = Vector3.zero;
//            var shortest = float.MaxValue;

//            for (int i = 0; i < count; i++)
//            {
//                var enemy = enemies[i];

//                var distance = (position - enemy.transform.position).sqrMagnitude;
//                if (distance < shortest)
//                {
//                    shortest = distance;
//                    nearest = enemy.transform.position;
//                }
//            }

//            if (nearest.sqrMagnitude == 0f)
//            {
//                return 0f;
//            }

//            var range = (position - nearest).magnitude;

//            return Mathf.Max(0f, (this.score - Mathf.Abs(this.desiredRange - range)));
//            //return 1f;  //  Return some calculated score.
//        }
//    }
//}