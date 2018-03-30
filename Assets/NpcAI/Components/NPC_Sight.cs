using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System.Collections;

using NpcBehavior;

public class NPC_Sight
{
    
    public Vector3 headPosition;
    public Vector3 targetPosition;
    public Vector3 dirToPlayer;                     //  Direction vector of npc amd target.
    public float angleBetweenNpcAndPlayer;

    

    NPC_BehaviorAI npc;
    NPC_Stats npcStats;

    public NPC_Sight(NPC_BehaviorAI _npc){
        npc = _npc;
        npcStats = npc.npcStats;
    }


    //  Updates all information.
    public void VisibilityCalculations(Transform target)
    {
        headPosition = npc.head.transform.position;  //npc.transform.position + Vector3.up * npc.offset;
        targetPosition = new Vector3(target.position.x, target.position.y + headPosition.y, target.position.z);

        dirToPlayer = (target.position - npc.transform.position).normalized;
        angleBetweenNpcAndPlayer = Vector3.Angle(npc.transform.forward, dirToPlayer);

    }


    //  Calculates if npc can see target.
    public bool CanSeeTarget(NPC_BehaviorAI npc, Transform target)
    {
        VisibilityCalculations(target);

        if (Vector3.Distance(headPosition, targetPosition) < npc.sightRange &&
           angleBetweenNpcAndPlayer < npc.viewAngle / 2f &&
           Physics.Linecast(npc.transform.position, target.position, npc.myObstacleLayer) == false)
        {
            npc.hasTargetInSight = true;
            return true;
        }
        npc.hasTargetInSight = false;
        return false;
    }



    //  Calculates the distance of the target to npc.  Compare the return value with sight range.
    public float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if(npc.myNavMeshAgent.enabled){
            npc.myNavMeshAgent.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = npc.transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++){
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++){
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }



}
