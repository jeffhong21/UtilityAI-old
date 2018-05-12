namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;

    [System.Serializable]
    public class RandomMove : ActionBase
    {
        
        float sightRange = 20f;  //  Make sure to consider navMesh stoppingDistance.

        protected override void Execute(IAIContext context)
        {
            var c = context as AIContext;

            Vector3 destination;

            if (RandomWanderTarget(c.entity.transform.position, sightRange, out destination)){
                c.entity.MoveTo(destination);
            }

        }



        bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            NavMeshHit navHit;
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * sightRange;


            if (NavMesh.SamplePosition(randomPoint, out navHit, range, NavMesh.AllAreas)){
                result = navHit.position;
                return true;
            }
            else{
                result = center;
                return false;
            }
        }


    }
}