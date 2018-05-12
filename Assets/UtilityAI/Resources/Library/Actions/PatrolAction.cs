namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;

    [Serializable]
    public class PatrolAction : ActionBase
    {


        protected override void Execute(IAIContext context)
        {
            AIContext c = context as AIContext;


            int index = UnityEngine.Random.Range(0, c.waypoints.Count - 1);
            Vector3 destination = c.waypoints[index].position;


            c.entity.MoveTo(destination);
        }




    }
}

