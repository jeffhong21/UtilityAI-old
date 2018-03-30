using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System.Collections;

using NpcBehavior;

public class NPC_Movement
{
    NPC_BehaviorAI npc;
    NPC_Stats npcStats;
    NPC_Sight npcSight;

    float minRange = 5;                //  The range for when the npc should move to target location.  If target is within range, npc will not move.


    public NPC_Movement(NPC_BehaviorAI _npc){
        npc = _npc;
        npcStats = npc.npcStats;
        npcSight = npc.npcSight;
    }


	public void MoveTo(Vector3 targetPos)
	{
        //  If character is too far from a waypoint, the character needs to go to a waypoint.
        if (Vector3.Distance(npc.transform.position, targetPos) >= minRange && npc.isDead == false){   
            npc.myNavMeshAgent.SetDestination(targetPos);
            npc.KeepWalking();
        }
	}

	public bool HaveReachedDestination(){
		if(npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance && npc.myNavMeshAgent.pathPending == false){
			npc.StopWalking();
			return true;
		} else {
			npc.KeepWalking();
			return false;
		}
	}




}
