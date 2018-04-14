namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    using Bang;

    /// <summary>
    /// Represents knowledge that the AI uses to do what it needs to do.
    /// </summary>
    [Serializable]
    public class AIContext : IAIContext
    {

        //[SerializeField]
        private NpcController _entity;
        public NpcController entity { get { return _entity; } private set { _entity = value; } }

        //[SerializeField]
        private NavMeshAgent _navMeshAgent;
        public NavMeshAgent navMeshAgent { get { return _navMeshAgent; } private set { _navMeshAgent = value; } }

        [SerializeField]
        private LayerMask _entitiesLayer;
        public LayerMask entitiesLayer { get { return _entitiesLayer; } private set { _entitiesLayer = value; } }


        public List<Transform> waypoints;  //{ get; private set; }

        public List<Transform> enemies;   //{ get; private set; }

        public List<Vector3> sampledPositions{ get; private set; }


        private List<GameObject> _entityObservations = new List<GameObject>();
        public List<GameObject> entityObservations { get { return _entityObservations; } private set { _entityObservations = value; } }


        public AIContext(NpcController entity)
        {
            this.entity = entity;
            this.enemies = new List<Transform>();
            this.sampledPositions = new List<Vector3>();


            entitiesLayer = (1 << LayerMask.NameToLayer("Entity"));

            waypoints = new List<Transform>();
            waypoints = GameObject.FindGameObjectsWithTag("Waypoints").Select(g => g.transform).ToList();

            navMeshAgent = entity.GetComponent<NavMeshAgent>();
            navMeshAgent.stoppingDistance = Math.Abs(navMeshAgent.stoppingDistance) < float.Epsilon ? 5f : navMeshAgent.stoppingDistance;
        }





    }
}








