﻿namespace UtilityAI
{
    using UnityEngine;

    public class HasEnemies : ScorerBase
    {
        [SerializeField]
        public float score = 25;


        public override float Score(IAIContext context)
        {
            var c = (AIContext)context;

            if (c.enemies.Count == 0)
            {
                //Debug.Log("No enemies");
                return 0f;
            }

            return this.score;
        }
    }
}
