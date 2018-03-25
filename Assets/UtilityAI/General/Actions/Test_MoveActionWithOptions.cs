namespace UtilityAI
{
    using UnityEngine;


    /// <summary>
    /// 
    /// </summary>
    public sealed class Test_MoveActionWithOptions : ActionWithOptions<Vector3>
    {


        public override void Execute(IContext context)
        {
            var c = (AIContext)context;

            //Debug.Log(this.DebugOptionsList(c.sampledPositions));

            Vector3 best = this.GetBest(c, c.sampledPositions);

            //Debug.Log(best);
            //  Move to the best position...
            if (System.Math.Abs(best.sqrMagnitude) < 0f) 
                return;



            //move to the position
            c.entity.MoveTo(best);

        }

    }
}