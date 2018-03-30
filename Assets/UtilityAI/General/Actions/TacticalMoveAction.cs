namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;



    public sealed class TacticalMoveAction : ActionWithOptions<Vector3>
    {


        protected override void Execute(IContext context)
        {
            var c = (AIContext)context;

            //Debug.Log(this.DebugOptionsList(c.sampledPositions));

            Vector3 bestDestination = this.GetBest(c, c.sampledPositions);

            //Debug.Log(best);
            //  Move to the best position...
            if (Mathf.Abs(bestDestination.sqrMagnitude) < 0f){
                EndAction();
                return;
            }
                



            //move to the position
            //c.entity.MoveTo(bestDestination);
            utilityAIComponent.StartCoroutine(MoveToDestination(c, bestDestination));

        }


        IEnumerator MoveToDestination(AIContext context, Vector3 destination)
        {
            bool hasReachedDestination = false;

            context.navMeshAgent.SetDestination(destination);
            //context.navMeshAgent.isStopped = false;

            while (hasReachedDestination == false)
            {
                if (context.navMeshAgent.pathPending == false)
                {
                    if (GetDistanceRemaining(context) <= context.navMeshAgent.stoppingDistance)
                    {
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
            for (int c = 0; c < corners.Length - 1; c++)
            {
                distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
            }
            return distance;
        }


        public TacticalMoveAction(params IOptionScorer<Vector3>[] objects) : base(objects)
        {
            
        }
            

    }
}