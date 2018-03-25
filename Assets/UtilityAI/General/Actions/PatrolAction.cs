namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;

    public class PatrolAction : ActionBase
    {



        public override void Execute(IContext context)
        {
            AIContext c = context as AIContext;
            NavMeshAgent navMeshAgent = c.entity.GetComponent<NavMeshAgent>();

            int index = UnityEngine.Random.Range(0, c.waypoints.Count - 1);
            Vector3 destination = c.waypoints[index].position;

            utilityAIComponent.StartCoroutine(MoveToDestination(c, destination));
            
        }


        public IEnumerator MoveToDestination(AIContext context, Vector3 destination)
        {
            bool hasReachedDestination = false;

            context.navMeshAgent.SetDestination(destination);
            //context.navMeshAgent.isStopped = false;

            while(hasReachedDestination == false)
            {
                // float distanceToTarget = Vector3.Distance(context.entity.position, destination);
                // if (distanceToTarget <= context.navMeshAgent.stoppingDistance)
                //     hasReachedDestination = true;
                // yield return null;
                if(context.navMeshAgent.pathPending == false){
                    if (context.navMeshAgent.remainingDistance <= context.navMeshAgent.stoppingDistance){
                        //context.navMeshAgent.isStopped = true; //  Stop walking.
                        hasReachedDestination = true;
                    }
                }
                else{
                    yield return null;
                }

                yield return null;
            }

            EndAction();
            yield return null;
        }


    }
}

