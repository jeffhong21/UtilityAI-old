namespace Bang
{
	using UnityEngine;
    using UnityEngine.AI;

	public class NpcMovement : MonoBehaviour 
	{

        private NavMeshAgent _navMeshAgent;


        private void Awake()
        {

            _navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            _navMeshAgent.enabled = true;
        }

        private void OnDisable()
        {
            _navMeshAgent.enabled = false;
        }


        public void MoveTo(Vector3 destination)
        {
            //Debug.Log(string.Format("Moving {0} to position {1}", this.gameObject.name, destination));
            _navMeshAgent.SetDestination(destination);
        }




	}
}