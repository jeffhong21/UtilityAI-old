using UnityEngine;
using UnityEngine.AI;

using NpcBehavior;


public class NPC_Alert : NPC_States
{
    NPC_BehaviorAI npc;
    NPC_Stats npcStats;
    NPC_Sight npcSight;
    NPC_Movement npcMove;
	
	float informRate = 3;			//  Rate at which to inform allies.
	float nextInform;
	int detectionCount;
	int lastDetectionCount;

	Collider[] friendlyColliders;
    Collider[] colliders;                   //  Used for gathering colliders around npc.
    Transform target;                       //  Npc's target.


	Color stateIndicatorColor = Color.yellow;

	public NPC_Alert(NPC_BehaviorAI npcStatePattern){
        npc = npcStatePattern; 
        npcSight = npc.npcSight;
        npcMove = npc.npcMove;
        npcStats = npc.npcStats;
	}

	public void UpdateState(){
        ScanForTargets();
	}

	public void ToPatrolState(){
        if(npc.debug) Debug.Log("Entering Patrol State");
		npc.myNavMeshAgent.speed = npcStats.baseSpeed;
        npc.pursueTarget = null;
		npc.possibleTarget = null;

		npc.currentState = npc.patrolState;
	}

	public void ToAlertState(){
		npc.currentState = npc.alertState;
	}

	public void ToPursueState(){
        if(npc.debug) Debug.Log("Going to Pursue State");
		npc.myNavMeshAgent.speed = npcStats.pursueSpeed;
		npc.possibleTarget = null;

		npc.currentState = npc.pursueState;
	}

	public void ToRangeAttackState(){

	}


    //  Set Location Of Interest.
    private void ScanForTargets()
    {
		lastDetectionCount = detectionCount;

        colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayers);

        foreach (Collider col in colliders)
        {
            RaycastHit hit;
            Transform target = col.transform;
            npcSight.VisibilityCalculations(target);
            
            if (npcSight.angleBetweenNpcAndPlayer < npc.viewAngle / 2f){
                if (Physics.Linecast(npcSight.headPosition, npcSight.targetPosition, out hit, npc.mySightLayers)){
                    foreach (string tags in npc.myEnemyTags){
                        if (hit.transform.CompareTag(tags)){
							detectionCount++;
							npc.possibleTarget = col.transform;
							break;
                        }
                    }
                }
            }
        }

		//  Check if detection count has changed and if not than set it back to 0.
		if(detectionCount == lastDetectionCount){
			detectionCount = 0;
		}

		//  Check if detection count is greater than the requirement and if so pursue.
		if(detectionCount >= npc.requiredDetectionCount){
			detectionCount = 0;
            npc.SetLocationOfInterest(npc.possibleTarget.position, true);
			npc.pursueTarget = npc.possibleTarget.root;

			InformNearbyAllies();
			ToPursueState();
		}

		GoToLocationOfInterest(npc.locationOfInterest);
    }



	void GoToLocationOfInterest(Vector3 locationOfInterest)
	{
		npc.UpdateStateIndicator(stateIndicatorColor);

		if(npc.myNavMeshAgent.enabled && locationOfInterest != Vector3.zero)
		{
			//Vector3 randomPosition = npc.RandomPositionAroundTarget(locationOfInterest);
			npcMove.MoveTo(locationOfInterest);

			if(npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance && npc.myNavMeshAgent.pathPending == false)
			{
				// npc.npcMaster.CallEventNpcIdleAnim();
                npc.SetLocationOfInterest(Vector3.zero, false);
				ToPatrolState();
			}
		}
	}



	void InformNearbyAllies()
	{
		// string alliesInformed = "";
		if (Time.time > nextInform){
			nextInform = Time.time + informRate;

			friendlyColliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myFriendlyLayers);

			if(friendlyColliders.Length == 0){
				return;
			}

			foreach(Collider ally in friendlyColliders){
				if(ally.transform.root.GetComponent<NPC_BehaviorAI>() != null){
					NPC_BehaviorAI allyPattern = ally.transform.root.GetComponent<NPC_BehaviorAI>();

					if(allyPattern.currentState == allyPattern.patrolState)
					{
						allyPattern.pursueTarget = npc.pursueTarget;
                        allyPattern.hasLocationOfInterest = true;
						allyPattern.locationOfInterest = npc.pursueTarget.position;
						allyPattern.currentState = allyPattern.alertState;
						// allyPattern.npcMaster.CallEventNpcWalkAnim();

						// alliesInformed += ally.gameObject.name + "\n";
						Debug.Log(npc.gameObject.name + " is informing " + ally.gameObject.name);
					}
				}
			}
		}

		// Debug.Log(alliesInformed);
	}








}
