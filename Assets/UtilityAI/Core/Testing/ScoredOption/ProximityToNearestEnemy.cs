namespace UtilityAI
{
    using UnityEngine;


    /// <summary>
    /// Distance from Player
    /// </summary>
    public sealed class ProximityToNearestEnemy : ScorerOptionBase<Vector3>
    {
        public float score = 25;
        public float desiredRange = 20;


        public override float Score(IAIContext context, Vector3 position)
        {
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

                var distance = (position - enemy.position).sqrMagnitude;
                if (distance < shortest)
                {
                    shortest = distance;
                    nearest = enemy.position;
                }
            }

            if (nearest.sqrMagnitude == 0f)
            {
                Debug.Log(string.Format("{0} is returning 0", this.GetType().Name));
                return 0f;
            }

            var range = (position - nearest).magnitude;
            return Mathf.Max(0f, (this.score - Mathf.Abs(this.desiredRange - range)));
        }
    }
}