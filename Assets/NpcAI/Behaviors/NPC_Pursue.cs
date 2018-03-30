using UnityEngine;
using UnityEngine.AI;

using NpcBehavior;


public class NPC_Pursue : NPC_States
{
    NPC_BehaviorAI npc;
    NPC_Stats npcStats;
    NPC_Sight npcSight;
    NPC_Movement npcMove;

    Collider[] colliders;                   //  Used for gathering colliders around npc.
    Transform target;                       //  Npc's target.

    float capturedDistance;

	Color stateIndicatorColor = Color.red;


	public NPC_Pursue(NPC_BehaviorAI npcStatePattern){
        npc = npcStatePattern; 
        npcSight = npc.npcSight;
        npcMove = npc.npcMove;
        npcStats = npc.npcStats;
	}


	public void UpdateState(){
        Look();
        Pursue();
	}


	public void ToPatrolState(){
		npc.myNavMeshAgent.speed = npcStats.baseSpeed;
		npc.pursueTarget = null;
		npc.currentState = npc.patrolState;
	}


	public void ToAlertState(){
		npc.myNavMeshAgent.speed = npcStats.baseSpeed;
		npc.currentState = npc.alertState;
	}


	public void ToPursueState(){
		npc.currentState = npc.pursueState;
	}


	public void ToRangeAttackState(){
		//npc.currentState = npc.rangeAttack;
	}


    void Look()
    {
		if(npc.pursueTarget == null){
			ToPatrolState();
			return;
		}

        colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange * npc.sightRangeMultiplier, npc.myEnemyLayers);

		if (colliders.Length == 0){
			npc.pursueTarget = null;
            ToAlertState();
			return;
		}

		capturedDistance = npc.sightRange * 2;

		foreach(Collider col in colliders){
			float distanceTotarg = Vector3.Distance(npc.transform.position, col.transform.position);

			if(distanceTotarg < capturedDistance)
			{
				capturedDistance = distanceTotarg;
				npc.pursueTarget = col.transform.root;
			}
		}


    }


    void Pursue(){

		npc.UpdateStateIndicator(stateIndicatorColor);
		

		if(npc.myNavMeshAgent.enabled && npc.pursueTarget != null){
			npcMove.MoveTo(npc.pursueTarget.position);
			npc.locationOfInterest = npc.pursueTarget.position;  // used by if npc goes to alert state.

			float distanceToTarget = Vector3.Distance(npc.transform.position, npc.pursueTarget.position);
			//  Check npc can see target and is in range.
			if(distanceToTarget <= npc.sightRange && npcSight.CanSeeTarget(npc, npc.pursueTarget)) {
				ToRangeAttackState();				
				npc.StopWalking();
			}
		} 
		else {
            ToAlertState();
		}
    }





}
