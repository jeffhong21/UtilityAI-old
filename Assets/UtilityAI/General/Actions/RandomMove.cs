namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;

    public class RandomMove : ActionBase
    {
        bool hasReachedDestination;
        float sightRange = 4f;

        public override void Execute(IContext context)
        {
            Debug.Log(string.Format("Executing action:  {0} | {1}", this.GetType().Name, Time.time));


            var c = (AIContext)context;
            Vector3 wanderLocation;


            if (RandomWanderTarget(c.entity.position, sightRange, out wanderLocation)){
                c.navMeshAgent.SetDestination(wanderLocation);
            }


            // while (hasReachedDestination == false || Time.time > Time.time + 10f)
            // {
            //     if (c.navMeshAgent.remainingDistance <= c.navMeshAgent.stoppingDistance && c.navMeshAgent.pathPending == false)
            //     {
            //         c.navMeshAgent.isStopped = true;  // Stop walking.
            //         hasReachedDestination = true;
            //     }
            //     else
            //     {
            //         c.navMeshAgent.isStopped = false;  // Keep walking.
            //         hasReachedDestination = false;
            //     }
            // }

        }




        bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            NavMeshHit navHit;

            Vector3 randomPoint = center + Random.insideUnitSphere * sightRange;
            if (NavMesh.SamplePosition(randomPoint, out navHit, 3.0f, NavMesh.AllAreas)){
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