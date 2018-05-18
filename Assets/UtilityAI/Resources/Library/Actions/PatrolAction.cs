namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Linq;

    [Serializable]
    public class PatrolAction : ActionBase
    {


        protected override void Execute(IAIContext context)
        {
            AIContext c = context as AIContext;

            if(c.waypoints.Count == 0){
                c.waypoints = GameObject.FindGameObjectsWithTag("Waypoints").Select(g => g.transform).ToList();
            }


            int index = UnityEngine.Random.Range(0, c.waypoints.Count - 1);
            Vector3 destination = c.waypoints[index].position;


            c.entity.MoveTo(destination);
        }




    }
}

