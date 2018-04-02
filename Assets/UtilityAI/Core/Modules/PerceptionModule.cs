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


    public class Module : IModule
    {

    }

    [Serializable]
    public class PerceptionModule
    {
        [Header("Detection Information")]
        public float viewAngle = 54f;
        public float sightRange = 20;                           //  How far can the enemy see.
        public float rangeAttackRange = 20;
        public int requiredDetectionCount = 15;
        public float sightRangeMultiplier = 1.5f;


        [Header("NPC Sight Infornation")]
        [Tooltip("What can the npc see.  Set this to everything except raycast, UI, etc.")]
        public LayerMask mySightLayers;
        [Tooltip("If Physics.Linecast hits anything in the LayerMask.  Set it as obstacles.")]
        public LayerMask myObstacleLayer;
        [Tooltip("What layers are considered the characters enemies.")]
        public LayerMask myEnemyLayers;
        [Tooltip("")]
        public LayerMask myFriendlyLayers;

        public string[] myEnemyTags;
        public string[] myFriendlyTags;





        ////  Calculates if npc can see target.
        //public bool CanSeeTarget(IContext context, Transform target)
        //{
        //    var headPosition = npc.head.transform.position;  //npc.transform.position + Vector3.up * npc.offset;
        //    var targetPosition = new Vector3(target.position.x, target.position.y + headPosition.y, target.position.z);

        //    var dirToPlayer = (target.position - npc.transform.position).normalized;
        //    var angleBetweenNpcAndPlayer = Vector3.Angle(npc.transform.forward, dirToPlayer);

        //    if (Vector3.Distance(headPosition, targetPosition) < npc.sightRange &&
        //       angleBetweenNpcAndPlayer < npc.viewAngle / 2f &&
        //       Physics.Linecast(npc.transform.position, target.position, npc.myObstacleLayer) == false)
        //    {
        //        npc.hasTargetInSight = true;
        //        return true;
        //    }
        //    npc.hasTargetInSight = false;
        //    return false;
        //}



        ////  Calculates the distance of the target to npc.  Compare the return value with sight range.
        //public float CalculatePathLength(Vector3 targetPosition)
        //{
        //    NavMeshPath path = new NavMeshPath();

        //    if (npc.myNavMeshAgent.enabled)
        //    {
        //        npc.myNavMeshAgent.CalculatePath(targetPosition, path);
        //    }

        //    Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        //    allWayPoints[0] = npc.transform.position;
        //    allWayPoints[allWayPoints.Length - 1] = targetPosition;

        //    for (int i = 0; i < path.corners.Length; i++)
        //    {
        //        allWayPoints[i + 1] = path.corners[i];
        //    }

        //    float pathLength = 0f;

        //    for (int i = 0; i < allWayPoints.Length - 1; i++)
        //    {
        //        pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        //    }

        //    return pathLength;
        //}



        ////  Set location of interest.
        //public void SetLocationOfInterest(Vector3 _position, bool _hasLocation)
        //{
        //    locationOfInterest = _position;
        //    hasLocationOfInterest = _hasLocation;
        //}


        //public Vector3 RandomPositionAroundTarget(Vector3 targetPosition)
        //{
        //    float offset = Random.Range(-10, 10);
        //    Vector3 originPos = targetPosition;
        //    originPos.x += offset;
        //    originPos.z += offset;

        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(originPos, out hit, 5, NavMesh.AllAreas))
        //    {
        //        return hit.position;
        //    }
        //    return targetPosition;
        //}


        ////  Get direction from angle.
        //public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal = false)
        //{
        //    if (angleIsGlobal == false)
        //    {
        //        angleInDegrees += transform.eulerAngles.y;
        //    }
        //    return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        //}




    }

}

