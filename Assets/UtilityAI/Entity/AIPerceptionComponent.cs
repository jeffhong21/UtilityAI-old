namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;
    using System.Collections.Generic;


    public class AIPerceptionComponent : MonoBehaviour
    {
        [Header("Agent Perception")]
        [Range(0, 360)]
        public float fieldOfView = 108f;
        public float sightRange = 20;
        public float yOffset = 1;

        [Header("Agent Sight Infornation")]
        [Tooltip("What can the npc see.  Set this to everything except raycast, UI, etc.")]
        public LayerMask sightLayer;
        [Tooltip("If Physics.Linecast hits anything in this LayerMask.  Set it as obstacles.")]
        public LayerMask obstaclesLayer;
        [Tooltip("What layers are considered the agents hostileEntities.")]
        public LayerMask hostileLayer;
        [Tooltip("What layers are considered the agents friendlies.")]
        public LayerMask friendlyLayer;

        public string[] hostileTags;
        public string[] friendlyTags;


        [Header("Agent Stats")]
        public int requiredDetectionCount = 15;
        public float sightRangeMultiplier = 1.5f;

        public float scanInterval = 0.2f;
        private float nextInterval;

        EntityAIController entity;
        TaskNetworkComponent taskNetwork;
        Transform sensor;


		private void Awake()
		{
            entity = GetComponent<EntityAIController>();
            sensor = entity.transform;
		}




		//private void Update()
		//{
  //          var time = Time.time;
  //          if (time - nextInterval < scanInterval){
  //              return;
  //          }
  //          nextInterval = time;

  //          var targets = new List<Transform>();
  //          var colliders = Physics.OverlapSphere(entity.transform.position, sightRange, sightLayer);


  //          foreach (Collider col in colliders)
  //          {
  //              Transform target = col.transform;

  //              if (CanSeeTarget(target))
  //              {
  //                  targets.Add(target);
  //              }
  //          }
		//}



		//  Set Location Of Interest.
		public void ScanForTargets(LayerMask layer, string[] tags, out List<Transform> data)
        {
            var targets = new List<Transform>();
            var colliders = Physics.OverlapSphere(entity.transform.position, sightRange, layer);


            foreach (Collider col in colliders)
            {
                Transform target = col.transform;

                if (CanSeeTarget(target)){
                    foreach (string tag in tags){
                        if (target.transform.CompareTag(tag)){
                            targets.Add(target);
                        }
                    }
                    //targets.Add(target);
                }
            }
            data = targets;
        }


		//  Calculates if npc can see target.
		public bool CanSeeTarget(Transform target)
		{
            var targetPosition = new Vector3(target.position.x, (target.position.y + sensor.position.y), target.position.z);
            var dirToPlayer = (target.position - entity.transform.position).normalized;

            var angleBetweenNpcAndPlayer = Vector3.Angle(entity.transform.forward, dirToPlayer);

            if (Vector3.Distance(sensor.position, targetPosition) < sightRange &&
		        angleBetweenNpcAndPlayer < fieldOfView / 2f &&
                Physics.Linecast(entity.transform.position, target.position, obstaclesLayer) == false)
		    {
		        return true;
		    }
		    return false;
		}




        //  Get direction from angle.
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal = false)
        {
            if (angleIsGlobal == false){
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }




        public Vector3 RandomPositionAroundTarget(Vector3 targetPosition, float range)
        {
            float offset = UnityEngine.Random.Range(-range, range);
            Vector3 originPos = targetPosition;
            originPos.x += offset;
            originPos.z += offset;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(originPos, out hit, 5, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return targetPosition;
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


        ////  Calculates if npc can see target.
        //public bool CanSeeTarget(IAIContext context, Transform target)
        //{
        //    var aiContext = context as AIContext;
        //    var entity = aiContext.entity;

        //    var targetPosition = new Vector3(target.position.x, (target.position.y + sensor.position.y), target.position.z);
        //    var dirToPlayer = (target.position - entity.transform.position).normalized;

        //    var angleBetweenNpcAndPlayer = Vector3.Angle(entity.transform.forward, dirToPlayer);

        //    if (Vector3.Distance(sensor.position, targetPosition) < sightRange &&
        //        angleBetweenNpcAndPlayer < fieldOfView / 2f &&
        //        Physics.Linecast(entity.transform.position, target.position, obstaclesLayer) == false)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
	}

}

