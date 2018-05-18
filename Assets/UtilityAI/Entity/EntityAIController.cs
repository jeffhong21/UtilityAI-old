namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;


    public class EntityAIController : MonoBehaviour
    {
        
        TaskNetworkComponent taskNetwork;
        AIContextProvider contextProvider;


        [HideInInspector]
        public AIPerceptionComponent aiSight;
        [HideInInspector]
        public EntityAISteering aiSteer;


        [HideInInspector]
        public bool isDead;





		private void Awake()
		{
            taskNetwork = GetComponent<TaskNetworkComponent>();
            contextProvider = GetComponent<AIContextProvider>();


            aiSight = GetComponent<AIPerceptionComponent>();


            aiSteer = GetComponent<EntityAISteering>();

		}


		private void Start()
		{
            
		}






		public void MoveTo(Vector3 destination)
        {
            AIContext context = contextProvider.GetContext() as AIContext;
            context.destination = destination;

            //aiMove.MoveTo(destination);
            aiSteer.MoveTo(destination);
        }

        public void StopMoving()
        {
            //aiMove.StopWalking();
            //Debug.Log(string.Format("{0} has stopped moving", this.gameObject.name));
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