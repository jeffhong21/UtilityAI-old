namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;

    public class PatrolAction : ActionBase
    {

        float _distance;


        protected override void Execute(IContext context)
        {
            AIContext c = context as AIContext;
            NavMeshAgent navMeshAgent = c.entity.GetComponent<NavMeshAgent>();

            int index = UnityEngine.Random.Range(0, c.waypoints.Count - 1);
            Vector3 destination = c.waypoints[index].position;

            utilityAIComponent.StartCoroutine(MoveToDestination(c, destination));
        }



        IEnumerator MoveToDestination(AIContext context, Vector3 destination)
        {
            bool hasReachedDestination = false;

            context.navMeshAgent.SetDestination(destination);
            //context.navMeshAgent.isStopped = false;

            while(hasReachedDestination == false)
            {
                if(context.navMeshAgent.pathPending == false){
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
            _distance = distance;
            return distance;
        }


    }
}

