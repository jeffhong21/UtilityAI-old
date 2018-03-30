//using UnityEngine;
//using UnityEngine.AI;

//using NpcBehavior;


//public class NPC_RangeAttack : NPC_States
//{
//    NPC_BehaviorAI npc;
//    NPC_Stats npcStats;
//    NPC_Sight npcSight;
//    NPC_Movement npcMove;

//    Collider[] colliders;                   //  Used for gathering colliders around npc.
//    Transform target;                       //  Npc's target.

//    float shotTime;
//    float shotInterval = 2f;

//    Color stateIndicatorColor = Color.cyan;

//    public NPC_RangeAttack(NPC_BehaviorAI npcStatePattern){
//        npc = npcStatePattern; 
//        npcSight = npc.npcSight;
//        npcMove = npc.npcMove;
//        npcStats = npc.npcStats;
//    }


//    public void UpdateState()
//    {
//        Look();
//        TryToAttack();
//    }

//    public void ToPatrolState()
//    {
//        npc.KeepWalking();
//        npc.myNavMeshAgent.speed = npcStats.baseSpeed;
//        npc.pursueTarget = null;
//        npc.hasTargetInSight = false;
//        npc.currentState = npc.patrolState;

//    }

//    public void ToAlertState()
//    {
//        npc.KeepWalking();
//        npc.hasTargetInSight = false;
//        npc.currentState = npc.alertState;
//    }

//    public void ToPursueState()
//    {
//        npc.KeepWalking();
//        npc.currentState = npc.pursueState;
//    }

//    public void ToRangeAttackState(){
//    }



//    void Look()
//    {
//        //  Return to PATROL state if there is no pursue targets.
//        if (npc.pursueTarget == null){
//            ToPatrolState();
//            return;
//        }

//        //  Check to see if enemy is in range.
//        Collider[] colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange * npc.sightRangeMultiplier, npc.myEnemyLayers);
//        // If there are no targets with range, return move to ALERT state.
//        if (colliders.Length == 0){
//            ToAlertState();
//            return;
//        }

//        foreach (Collider col in colliders){
//            //  If collider is equal to pursue target, try to attack.
//            if (col.transform.root == npc.pursueTarget){
//                return;
//            }
//        }

//        // TODO:  why am I moving to pursue state?  If target is not in sight it will move to pursue state.
//        ToPursueState();

//    }


//    void TryToAttack(){
//        if (npc.pursueTarget != null)
//        {
//            npc.UpdateStateIndicator(stateIndicatorColor);

//            if (npcSight.CanSeeTarget(npc, npc.pursueTarget) && Time.time > npc.nextAttack){      // If npc can see target.
//                npc.nextAttack = Time.time + npcStats.attackRate;

//                //  Rotate npc towards player.
//                Vector3 newPos = new Vector3(npc.pursueTarget.position.x, npc.pursueTarget.position.y, npc.pursueTarget.position.z);
//                npc.transform.LookAt(newPos);

//                Shoot();

//                // if(distanceToTarget <= npc.meleeAttackRange && npc.hasMeleeAttack){
//                // 	ToMeleeAttackState();
//                // }
                
//            }

//            else if (npcSight.CanSeeTarget(npc, npc.pursueTarget) == false){
//                ToPursueState();
//                return;
//            }

//        }
//        else
//        {
//            ToPatrolState();
//        }
//    }


//    void Shoot(){

//        float distanceToTarget = Vector3.Distance(npc.transform.position, npc.pursueTarget.position);
        
//        if (distanceToTarget <= npc.rangeAttackRange && 
//            Time.time > shotTime &&
//            npc.rangeWeapon.GetAmmoRemaining() > 0)
//        {
//            shotTime = Time.time + shotInterval;  //  Reset shot time.

//            npc.StopWalking();
//            //Debug.Log (string.Format("Shooting {0}", npc.pursueTarget.name) );
//            npc.rangeWeapon.Shoot();
//            npc.rangeWeapon.CockGun();
//        }

//        else if (npc.rangeWeapon.GetAmmoRemaining() == 0)
//        {
//            Debug.Log(string.Format("{0} is currently reloading.", npc.name));
//            npc.rangeWeapon.Reload();
//        }
//    }




//}
