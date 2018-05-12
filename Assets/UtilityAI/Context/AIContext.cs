namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    


    /// <summary>
    /// Represents knowledge that the AI uses to do what it needs to do.
    /// </summary>
    [Serializable]
    public class AIContext : IAIContext
    {

        [SerializeField]
        private AIEntityController _entity;
        public AIEntityController entity { get { return _entity; } private set { _entity = value; } }

        [SerializeField]
        private Vector3 _destination;
        public Vector3 destination { get { return _destination; } set { _destination = value; } }

        [SerializeField]
        private Vector3 _locationOfInterest;
        public Vector3 locationOfInterest { get { return _locationOfInterest; } set { _locationOfInterest = value; } }

        [SerializeField]
        private Transform _focusTarget;
        public Transform focusTarget { get { return _focusTarget; } set { _focusTarget = value; } }

        [SerializeField]
        private List<Transform> _waypoints = new List<Transform>();
        public List<Transform> waypoints { get { return _waypoints; } set { _waypoints = value; } }

        [SerializeField]
        private List<Transform> _hostileEntities = new List<Transform>();
        public List<Transform> hostileEntities { get { return _hostileEntities; } set { _hostileEntities = value; } }

        [SerializeField]
        private List<Vector3> _tacticalPositions = new List<Vector3>();
        public List<Vector3> tacticalPositions { get { return _tacticalPositions; } set { _tacticalPositions = value; } }

        //[SerializeField]
        private List<GameObject> _friendlyEntities = new List<GameObject>();
        public List<GameObject> friendlyEntities { get { return _friendlyEntities; } set { _friendlyEntities = value; } }



        [SerializeField]
        private LayerMask _entitiesLayer;
        public LayerMask entitiesLayer { get { return _entitiesLayer; } private set { _entitiesLayer = value; } }


        public NavMeshAgent navMeshAgent;





        public AIContext(AIEntityController aIEntity)
        {
            Debug.Log("Initializing Context");
            entity = aIEntity;

            navMeshAgent = entity.GetComponent<NavMeshAgent>();
            navMeshAgent.stoppingDistance = Math.Abs(navMeshAgent.stoppingDistance) < float.Epsilon ? 5f : navMeshAgent.stoppingDistance;


            waypoints = new List<Transform>();
            waypoints = GameObject.FindGameObjectsWithTag("Waypoints").Select(g => g.transform).ToList();

            entitiesLayer = (1 << LayerMask.NameToLayer("Entity"));
        }






    }
}








