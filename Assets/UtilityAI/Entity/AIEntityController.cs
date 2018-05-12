namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(NavMeshAgent))]
    public class AIEntityController : MonoBehaviour
    {
        
        TaskNetworkComponent taskNetwork;
        AIContextProvider contextProvider;

        [HideInInspector]
        public AIEntityMoveModule move;
        [HideInInspector]
        public AIEntityPerceptionModule sight;
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public bool isDead;



        void InitializeReferences()
		{
            taskNetwork = GetComponent<TaskNetworkComponent>();
            contextProvider = GetComponent<AIContextProvider>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            move = GetComponent<AIEntityMoveModule>();
            sight = GetComponent<AIEntityPerceptionModule>();
		}

		private void Awake()
		{
            InitializeReferences();
		}


		private void Start()
		{
            navMeshAgent.stoppingDistance = Mathf.Abs(navMeshAgent.stoppingDistance) < float.Epsilon ? 5f : navMeshAgent.stoppingDistance;
		}




		public void MoveTo(Vector3 destination)
        {
            AIContext context = contextProvider.GetContext() as AIContext;
            context.destination = destination;
            move.MoveToDestination(destination);
        }


        public void StopMoving()
        {
            move.StopWalking();
            Debug.Log(string.Format("{0} has stopped moving", this.gameObject.name));
        }

        public void Shoot()
        {
            Debug.Log(string.Format("{0} is shooting", this.gameObject.name));
        }

        public void Reload()
        {
            Debug.Log(string.Format("{0} is reloading", this.gameObject.name));
        }

        public void AddAmmo(int _amount)
        {
            Debug.Log(string.Format("{0} is adding ammo", this.gameObject.name));
        }

        public void OnDeath()
        {
            //  Turn off the movement and shooting scripts.
        }

        protected virtual void OnAttackTargetChange()
        {
            //  When a new attack target is set, we want to turn towards the target.
        }

        protected virtual void OnAttackTargetDead()
        {
            //  When target dies, stop shooting

        }
    }
}