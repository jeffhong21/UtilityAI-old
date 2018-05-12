namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;


    public class AIEntityMoveModule : MonoBehaviour
    {
        public float moveSpeed = 8;
        public float turnSpeed;
        public float radius;

        AIEntityController entity;
        NavMeshAgent navMeshAgent;
        float minRange = 5;      //  The range for when the entity should move to target location.  If target is within range, npc will not move.


		private void Awake()
		{
            InitializeReferences();
		}

		private void OnEnable()
		{
			
		}


		void InitializeReferences()
        {
            if(entity == null) entity = GetComponent<AIEntityController>();
            if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
        }



        public void MoveToDestination(Vector3 destination)
        {
            //  If character is too far from a waypoint, the character needs to go to a waypoint.
            if (Vector3.Distance(entity.transform.position, destination) >= minRange && entity.isDead == false){
                //Debug.Log(string.Format("Moving {0} to position {1}", this.gameObject.name, destination));
                navMeshAgent.SetDestination(destination);
                KeepWalking();
            }
        }


        //public void MoveToPoint(Vector3 destination)
        //{
        //    Vector3 direction = entity.transform.DirectionTo(destination);
        //    float distance = Vector3.Distance(destination, entity.transform.position);
        //    if (distance > 0.1f)
        //    {
        //        LookToward(destination, distance);
        //        float distanceBaseSpeedModifier = GetSpeedModifier(distance);
        //        Vector3 movement = entity.transform.forward * Time.deltaTime * distanceBaseSpeedModifier;
        //        navMeshAgent.Move(movement);
        //    }
        //}


        public bool HaveReachedDestination()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && navMeshAgent.pathPending == false){
                StopWalking();
                return true;
            }
            else{
                KeepWalking();
                return false;
            }
        }


        bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            float sightRange = entity.sight.sightRange;
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





        public void KeepWalking(){
            navMeshAgent.isStopped = false;
        }

        public void StopWalking(){
            navMeshAgent.isStopped = true;
        }

    }
}