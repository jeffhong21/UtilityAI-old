using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System.Collections;

using Bang;

namespace NpcBehavior
{
    public class NPC_BehaviorAI : MonoBehaviour
    {
        public NPC_Stats npcStats;
        //[Header("Weapons")]
        //public Gun rangeWeapon;

        [Header("Detection Information")]
        public float viewAngle = 54f;
		public float sightRange = 20;							//  How far can the enemy see.
		public float rangeAttackRange = 20;	
		public int requiredDetectionCount = 15;
        public float sightRangeMultiplier = 1.5f;
        private float checkRate = 0.1f;                         //  Used in Update() to increase performance so it doesn't check every frame   
        private float nextCheck;    

		[Header("NPC Sight Infornation")]
		public LayerMask mySightLayers;							//  What can the npc see.  Set this to everything except raycast, UI, etc.
		public LayerMask myObstacleLayer;  						//  If Physics.Linecast hits anything in the LayerMask.  Set it as obstacles.
		public LayerMask myEnemyLayers;							//  What layers are considered the characters enemies.
		public LayerMask myFriendlyLayers;
		public string[] myEnemyTags;
		public string[] myFriendlyTags;


		[Header("Location Information")]
		public Transform pursueTarget;                          //  Attack target of the npc.
        public Transform possibleTarget;                        //  Used for Alert state.
		[HideInInspector] public Vector3 locationOfInterest;    //  Can be used when distracting npc.               
        [HideInInspector] public Vector3 lastKnownLocation;     //  Last known position after the npc has seen target.
		[HideInInspector] public Vector3 wanderLocation;        //  Next position npc is going to move to.
        public bool hasLocationOfInterest;                      //  Used to indicate something has caught the npc's eye.
        public bool hasTargetInSight;


		[Header("Character Reference Information")]
		public MeshRenderer stateIndicator;
		public Transform head;
        public Transform weaponHold;

        [Header("Character Reference Information")]
        public bool debug;

        [HideInInspector] public bool isDead;
        [HideInInspector] public int health;
        [HideInInspector] public float nextAttack;				//  Used to indicate the next attack


        [HideInInspector] public NavMeshAgent myNavMeshAgent;
        [HideInInspector] public NPC_Sight npcSight;
        [HideInInspector] public NPC_Movement npcMove;
		

		[HideInInspector] public NPC_States currentState;
		[HideInInspector] public NPC_States capturedState;
		[HideInInspector] public NPC_Patrol patrolState;
		[HideInInspector] public NPC_Alert alertState;
		[HideInInspector] public NPC_Pursue pursueState;
		//[HideInInspector] public NPC_RangeAttack rangeAttack;




		void Awake(){
            //  Setup script references
			SetInitialReferences();
            //  Setup State references.
            SetupUpStateReferences();
		}


        void Start(){
			SetInitialReferences();
            //if(rangeWeapon != null) EquipWeapon(rangeWeapon, "Revolver");

			myNavMeshAgent.updatePosition = true;	
			myNavMeshAgent.updateRotation = true;
			myNavMeshAgent.stoppingDistance = 2;

            health = npcStats.startingHealth;
			myNavMeshAgent.speed = npcStats.baseSpeed;
        }

		void SetInitialReferences()
		{
            npcSight = new NPC_Sight(this);
            npcMove = new NPC_Movement(this);
            npcStats = new NPC_Stats(); //GetComponent<NPC_Stats>();

            myNavMeshAgent = GetComponent<NavMeshAgent>();

			currentState = patrolState;
		}


		void SetupUpStateReferences()
		{
			patrolState = new NPC_Patrol(this);
			alertState = new NPC_Alert(this);
			pursueState = new NPC_Pursue(this);
			//rangeAttack = new NPC_RangeAttack(this);
		}


        void Update()
        {
			if(Time.time > nextCheck){
				nextCheck = Time.time + checkRate;
                currentState.UpdateState();
			}
            
        }


        //  Set location of interest.
        public void SetLocationOfInterest(Vector3 _position, bool _hasLocation)
        {
            locationOfInterest = _position;
            hasLocationOfInterest = _hasLocation;
        }


        public void UpdateStateIndicator(Color _color){
            stateIndicator.sharedMaterial.color = _color;
        }


        public void KeepWalking(){
            myNavMeshAgent.isStopped = false;
        }


        public void StopWalking(){
            myNavMeshAgent.isStopped = true;
        }


        //public void EquipWeapon(Gun gunToEquip, string gunName = ""){
        //    // Debug.Log("Equipping " + gunToEquip);
        //    rangeWeapon = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        //    rangeWeapon.transform.parent = weaponHold;
        //}



        public Vector3 RandomPositionAroundTarget(Vector3 targetPosition){
            float offset = Random.Range(-10, 10);
            Vector3 originPos = targetPosition;
            originPos.x += offset;
            originPos.z += offset;

            NavMeshHit hit;
            if(NavMesh.SamplePosition(originPos, out hit, 5, NavMesh.AllAreas)){
                return hit.position;
            }
            return targetPosition;
        }


        //  Get direction from angle.
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal = false)
        {
            if (angleIsGlobal == false){
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }




        

    }


    public interface NPC_States
    {
        void UpdateState();
        void ToPatrolState();
        void ToAlertState();
        void ToPursueState();
        //void ToMeleeAttackState();
        void ToRangeAttackState();
    }

}