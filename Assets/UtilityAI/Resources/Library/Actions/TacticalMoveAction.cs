namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections;


    [System.Serializable]
    public sealed class TacticalMoveAction : ActionWithOptions<Vector3>
    {

        public TacticalMoveAction(params IOptionScorer<Vector3>[] objects) : base(objects)
        {}


        protected override void Execute(IAIContext context)
        {
            var c = (AIContext)context;
            Vector3 bestDestination = GetBest(c, c.tacticalPositions);

            //  Move to the best position...
            if (Mathf.Abs(bestDestination.sqrMagnitude) < 0f){
                Debug.Log("Did not get a best destination");
                //EndAction();
                return;
            }


            if(c.navMeshAgent != null && Mathf.Abs(bestDestination.sqrMagnitude) > Mathf.Abs(c.entity.transform.position.sqrMagnitude) )
            {
                Debug.Log(string.Format("Entity position:  <{0}>  |  Entity sqrMagnitude:  {1}\nBestDestination position:  <{2}>  |  BestDestination sqrMagnitude:  {3}", 
                                        c.entity.transform.position, Mathf.Abs(c.entity.transform.position.sqrMagnitude),bestDestination, Mathf.Abs(bestDestination.sqrMagnitude)));
                
                c.entity.MoveTo(bestDestination);
                //MoveTo(c.navMeshAgent, bestDestination);
            }
            else{
                //Debug.Log(string.Format("Entity position:  {0}\nBest Destination: {1}", c.entity.transform.position, bestDestination));
                return;
            }

        }





        IEnumerator MoveToDestination(AIContext context, Vector3 destination)
        {
            bool hasReachedDestination = false;

            context.entity.navMeshAgent.SetDestination(destination);
            //context.entity.navMeshAgent.isStopped = false;

            while (hasReachedDestination == false)
            {
                if (context.entity.navMeshAgent.pathPending == false && GetDistanceRemaining(context) <= context.entity.navMeshAgent.stoppingDistance){
                    //context.entity.navMeshAgent.isStopped = true; //  Stop walking.
                    hasReachedDestination = true;
                }
                yield return null;
            }

            //EndAction();
            yield return null;
        }


        float GetDistanceRemaining(AIContext context)
        {
            float distance = 0.0f;
            Vector3[] corners = context.entity.navMeshAgent.path.corners;
            for (int c = 0; c < corners.Length - 1; c++)
            {
                distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
            }
            return distance;
        }



            

    }
}