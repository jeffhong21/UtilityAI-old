namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;
    using System.Collections.Generic;


    public interface IModule
    {
        
    }


    [Serializable]
    public class PerceptionModule
    {
        [Header("Detection Information")]
        public float viewAngle = 108f;
        public float sightRange = 20;                           //  How far can the enemy see.
        public float rangeAttackRange = 20;
        public int requiredDetectionCount = 15;
        public float sightRangeMultiplier = 1.5f;


        [Header("Agent Sight Infornation")]
        [Tooltip("What can the npc see.  Set this to everything except raycast, UI, etc.")]
        public LayerMask mySightLayers;
        [Tooltip("If Physics.Linecast hits anything in this LayerMask.  Set it as obstacles.")]
        public LayerMask myObstacleLayer;
        [Tooltip("What layers are considered the agents enemies.")]
        public LayerMask myEnemyLayers;
        [Tooltip("What layers are considered the agents friendlies.")]
        public LayerMask myFriendlyLayers;

        public string[] myEnemyTags;
        public string[] myFriendlyTags;




        private UtilityAIComponent utilityAIComponent;
        private GameObject entity;
        private Transform sensor;


        public void InitializePerceptionModule(UtilityAIComponent uai)
		{
            utilityAIComponent = uai;
            entity = uai.gameObject;
            sensor = entity.transform;
		}



		//  Calculates if npc can see target.
		public bool CanSeeTarget(Transform target)
		{
            var targetPosition = new Vector3(target.position.x, (target.position.y + sensor.position.y), target.position.z);
            var dirToPlayer = (target.position - entity.transform.position).normalized;

            var angleBetweenNpcAndPlayer = Vector3.Angle(entity.transform.forward, dirToPlayer);

            if (Vector3.Distance(sensor.position, targetPosition) < sightRange &&
		        angleBetweenNpcAndPlayer < viewAngle / 2f &&
                Physics.Linecast(entity.transform.position, target.position, myObstacleLayer) == false)
		    {
		        return true;
		    }
		    return false;
		}


        //  Get direction from angle.
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal = false)
        {
            if (angleIsGlobal == false){
                angleInDegrees += entity.transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }


        //  Calculates the distance of the target to npc.  Compare the return value with sight range.
        public float CalculatePathLength(NavMeshAgent navMeshAgent, Vector3 targetPosition)
        {
            NavMeshPath path = new NavMeshPath();

            if (navMeshAgent.enabled)
            {
                navMeshAgent.CalculatePath(targetPosition, path);
            }

            Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

            allWayPoints[0] = entity.transform.position;
            allWayPoints[allWayPoints.Length - 1] = targetPosition;

            for (int i = 0; i < path.corners.Length; i++)
            {
                allWayPoints[i + 1] = path.corners[i];
            }

            float pathLength = 0f;

            for (int i = 0; i < allWayPoints.Length - 1; i++)
            {
                pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
            }

            return pathLength;
        }



        ////  Set location of interest.
        //public void SetLocationOfInterest(Vector3 _position, bool _hasLocation)
        //{
        //    locationOfInterest = _position;
        //    hasLocationOfInterest = _hasLocation;
        //}


        //public Vector3 RandomPositionAroundTarget(Vector3 targetPosition)
        //{
        //    float offset = UnityEngine.Random.Range(-10, 10);
        //    Vector3 originPos = targetPosition;
        //    originPos.x += offset;
        //    originPos.z += offset;

        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(originPos, out hit, 5, NavMesh.AllAreas)){
        //        return hit.position;
        //    }
        //    return targetPosition;
        //}















	}

}

