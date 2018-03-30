using UnityEngine;
using UnityEngine.AI;

using NpcBehavior;


public class NPC_Patrol : NPC_States
{
    NPC_BehaviorAI npc;
    NPC_Stats npcStats;
    NPC_Sight npcSight;
    NPC_Movement npcMove;

	Transform[] waypoints = new Transform[0];
	int nextWaypoint = 0;

    Collider[] colliders;                   //  Used for gathering colliders around npc.
    Transform target;                       //  Npc's target.

    Color stateIndicatorColor = Color.green;

	public NPC_Patrol(NPC_BehaviorAI npcStatePattern){
        npc = npcStatePattern; 
        npcSight = npc.npcSight;
        npcMove = npc.npcMove;
        npcStats = npc.npcStats;
	}

	public void UpdateState(){
        ScanForTargets();
        Patrol();
	} 

	public void ToPatrolState(){
	}

	public void ToAlertState(){
        if(npc.debug) Debug.Log("Entering Alert State");
		npc.currentState = npc.alertState;
	}

	public void ToPursueState(){
	}

	public void ToRangeAttackState(){
	}


    //  Set Location Of Interest.
    void ScanForTargets()
    {
        npc.UpdateStateIndicator(stateIndicatorColor);

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
                            Debug.DrawLine(npcSight.headPosition, npcSight.targetPosition, Color.red);
                            AlertStateActions(target);
                            return;
                        }
                    }
                }
            }
        }
    }



	void AlertStateActions(Transform target){
        npc.SetLocationOfInterest(target.position, true);
		ToAlertState();
	}


	void Patrol()
	{
        if (waypoints.Length > 0)
        {
            npcMove.MoveTo(waypoints[nextWaypoint].position);
            if (npcMove.HaveReachedDestination()){
                nextWaypoint = Random.Range(0, waypoints.Length);
            }
        }

        else{       //  Wander about if there are no waypoints
            if (npcMove.HaveReachedDestination()){
                npc.StopWalking();
                if (RandomWanderTarget(npc.transform.position, npc.sightRange, out npc.wanderLocation)){
                    npcMove.MoveTo(npc.wanderLocation);
                }
            }
        }
	}


    //  Return a random location to wander too.
	bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
	{
		NavMeshHit navHit;

		Vector3 randomPoint = center + Random.insideUnitSphere * npc.sightRange;
		if(NavMesh.SamplePosition(randomPoint, out navHit, 3.0f, NavMesh.AllAreas)){
			result = navHit.position;
			return true;
		}
		else{
			result = center;
			return false;
		}
	}







}
