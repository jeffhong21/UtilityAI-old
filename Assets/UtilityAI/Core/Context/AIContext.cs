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
    public class AIContext : IContext
    {
        public AIContext(NpcController entity)
        {
            this.entity = entity;
            this.navMeshAgent = entity.GetComponent<NavMeshAgent>();

            this.waypoints = new List<Transform>();
            this.enemies = new List<IEntity>();
            this.sampledPositions = new List<Vector3>();
            
            waypoints = GameObject.FindGameObjectsWithTag("Waypoints").Select(g => g.transform).ToList();

            navMeshAgent.stoppingDistance = navMeshAgent.stoppingDistance == 0f ? 5f : navMeshAgent.stoppingDistance;
        }


        [SerializeField]
        private NpcController _entity;
        public NpcController entity { get { return _entity; } private set { _entity = value; } }


        public NavMeshAgent navMeshAgent;

        public List<Transform> waypoints;  //{ get; private set; }

        public List<IEntity> enemies;   //{ get; private set; }

        public List<Vector3> sampledPositions;   //{ get; private set; }


    }
}








