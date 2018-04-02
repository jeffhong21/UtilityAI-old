namespace UtilityAI
{

    using UnityEngine;

    public class HasAttackTarget : ScorerBase
    {

        [SerializeField]
        public float score = 25;

        public override float Score(IContext context)
        {
            
            //var c = (AIContext)context;

            //if (c.entity.attackTarget == null)
            //{
            //    return this.score;
            //}

            return 0f;
        }
    }
}
