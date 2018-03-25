namespace UtilityAI
{
    using UnityEngine;

    public class IsGunLoaded : ScorerBase
    {
        [SerializeField]
        public float score = 40;

        public override float Score(IContext context)
        {
            //var c = (AIContext)context;

            //var player = c.entity;

            //if (player.currentAmmo <= 0)
            //{
            //    return this.score;
            //}

            return 0f;
        }
    }
}