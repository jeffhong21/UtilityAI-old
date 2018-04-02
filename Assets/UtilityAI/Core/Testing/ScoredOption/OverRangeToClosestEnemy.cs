namespace UtilityAI
{
    using UnityEngine;


    /// <summary>
    /// Distance from Player
    /// </summary>
    public sealed class OverRangeToClosestEnemy : ScorerOptionBase<Vector3>
    {
        public float score = 25;
        public float desiredRange = 20;


        public override float Score(IContext context, Vector3 position)
        {
            /* Put logic here */

            var c = context as AIContext;

            var enemies = c.enemies;
            var count = enemies.Count;
            if (count == 0)
            {
                return 0f;
            }

            var nearest = Vector3.zero;
            var shortest = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var enemy = enemies[i];

                var distance = (position - enemy.gameObject.transform.position).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    nearest = enemy.gameObject.transform.position;
                }
            }

            //if (nearest.sqrMagnitude > float.Epsilon)
            //{
            //    return 0f;
            //}

            var range = (position - nearest).magnitude;

            if (range > desiredRange){
                return this.score;
            }
            else{
                return 0;
            }
            //return Mathf.Max(0f, (this.score - Mathf.Abs(this.desiredRange - range)));
            //return 1f;  //  Return some calculated score.
        }
    }
}