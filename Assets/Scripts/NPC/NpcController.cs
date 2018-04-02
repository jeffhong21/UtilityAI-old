namespace Bang
{
	using UnityEngine;
    using UtilityAI;

    public class NpcController : Entity
	{
        //public float scanRange = 20f;
		
		

		//  Context
        //private AIContext npcContext;
		//private NpcHealth npcHealth;
		//private NpcShoot npcShoot;
		//private NpcMovement npcMove;
		//  AI
        private UtilityAIComponent npcAI;

		void Awake()
		{
            //npcContext = new AIContext(this);
			//npcHealth = this.GetComponent<NpcHealth>();
			//npcShoot = this.GetComponent<NpcShoot>();
			//npcMove = this.GetComponent<NpcMovement>();
            npcAI = this.GetComponent<UtilityAIComponent>();
		}

		void OnEnable(){
			//npcHealth.enabled = true;
			//npcShoot.enabled = true;
			//npcMove.enabled = true;
		}

		void OnDisable()
		{
			
		}

        //public IContext GetContext(){
        //    return npcContext as IContext;
        //}

		public void MoveTo(Vector3 _position){
			
            //npcMove.MoveTo(_position);
		}

		public void StopMoving(){
			Debug.Log(string.Format("{0} has stopped moving", this.gameObject.name));
		}

		public void Shoot(){
			Debug.Log(string.Format("{0} is shooting", this.gameObject.name));
		}

		public void Reload(){
			Debug.Log(string.Format("{0} is reloading", this.gameObject.name));
		}

		public void AddAmmo(int _amount){
			Debug.Log(string.Format("{0} is adding ammo", this.gameObject.name));
		}

		public void OnDeath(){
			//  Turn off the movement and shooting scripts.
		}

		protected virtual void OnAttackTargetChange(){
			//  When a new attack target is set, we want to turn towards the target.
		}

		protected virtual void OnAttackTargetDead(){
			//  When target dies, stop shooting

		}
	}
}