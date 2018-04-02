namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;

    public class RandomMove : ActionBase
    {
        
        float sightRange = 20f;  //  Make sure to consider navMesh stoppingDistance.

        protected override void Execute(IContext context)
        {
            var c = context as AIContext;

            Vector3 destination;

            if (RandomWanderTarget(c.entity.position, sightRange, out destination)){
                utilityAIComponent.StartCoroutine(MoveToDestination(c, destination));
            }else{
                EndAction();
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



        IEnumerator MoveToDestination(AIContext context, Vector3 destination)
        {
            bool hasReachedDestination = false;

            context.navMeshAgent.SetDestination(destination);
            //context.navMeshAgent.isStopped = false;
            //Debug.Log(string.Format("Entities Location: <{0}> |Destination:  <{1}>", context.entity.position, destination));
            while (hasReachedDestination == false)
            {
                if (context.navMeshAgent.pathPending == false){
                    if (GetDistanceRemaining(context) <= context.navMeshAgent.stoppingDistance){
                        //context.navMeshAgent.isStopped = true; //  Stop walking.
                        hasReachedDestination = true;
                    }
                }


                yield return null;
            }


            EndAction();
            yield return null;
        }


        float GetDistanceRemaining(AIContext context)
        {
            float distance = 0.0f;
            Vector3[] corners = context.navMeshAgent.path.corners;
            for (int c = 0; c < corners.Length - 1; c++){
                distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
            }
            return distance;
        }

    }
}