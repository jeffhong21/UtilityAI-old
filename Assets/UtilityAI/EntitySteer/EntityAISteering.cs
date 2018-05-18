namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EntityAIPerception))]
    public class EntityAISteering : MonoBehaviour
    {
        [Header("Movement")]
        public float speed = 6f;
        public float acceleration = 8f;
        [Header("Rotation")]
        public float angularSpeed = 270f;
        public float angularAcceleration = 40f;
        [Header("Misc")]
        public float arrivalDistance = 1f;

        [HideInInspector]
        public NavMeshAgent agent;
        [HideInInspector]
        public NavMeshPath path;
        [SerializeField]
        bool hasPath;
        Queue<Vector3> cornerQueue;
        Vector3 currentDestination;



		void Awake()
		{
            if (agent == null) agent = GetComponent<NavMeshAgent>();
		}

        void OnEnable()
        {
            agent.speed = speed;
            agent.acceleration = acceleration;
            agent.angularSpeed = angularSpeed;
            agent.stoppingDistance = arrivalDistance;
            agent.autoBraking = false;
            // we give the unit a random avoidance priority so as to ensure that units will actually avoid each other (since same priority units will not try to avoid each other)
            agent.avoidancePriority = UnityEngine.Random.Range(0, 99);

            path = new NavMeshPath();
            cornerQueue = new Queue<Vector3>();
            agent.enabled = true;
        }

        void OnDisable()
        {
            path = null;
            cornerQueue = null;
            agent.enabled = false;
        }

        void Update()
        {
            if(hasPath == false){
                return;
            }

            if (hasPath)
            {
                Vector3 steering = Vector3.zero;
                Vector3 followForce = GetTargetVector(GetPosition(), currentDestination);
                steering += followForce;

                //Vector3 separationForce = GetSeparationVector(separationDistance, 20f);
                //steering += separationForce;

                Vector3 targetDestination = transform.position + steering;
                agent.SetDestination(targetDestination);
            }


            //if (hasPath && GetDistanceRemaining() <= arrivalDistance)
            //{
            //    Vector3 steering = Vector3.zero;
            //    Vector3 followForce = GetTargetVector(GetPosition(), currentDestination);
            //    steering += followForce;
            //    //Vector3 separationForce = GetSeparationVector(separationDistance, 20f);
            //    //steering += separationForce;

            //    Vector3 targetDestination = transform.position + steering;
            //    agent.SetDestination(targetDestination);
            //}

            //else if (hasPath && GetDistanceRemaining() >= arrivalDistance)
            //{
            //    Vector3 toTarget = agent.steeringTarget - transform.position;
            //    float turnAngle = Vector3.Angle(transform.forward, toTarget);
            //    turnAngle = Mathf.Clamp(turnAngle, acceleration, angularAcceleration);
            //    agent.acceleration = turnAngle;
            //}

            //if (hasPath)
            //{
            //    var distRemaining = (currentDestination - transform.position).sqrMagnitude;
            //    if (distRemaining > arrivalDistance)
            //    {
            //        RotateTowards(currentDestination);
            //        Vector3 velocity = transform.forward * Time.deltaTime * acceleration;
            //        velocity.y = 0f;
            //        agent.Move(velocity);
            //    }
            //    else{
            //        GetNextCorner();
            //    }
            //}


            if (HasReachedDestination())
            {
                //agent.acceleration = acceleration;
                hasPath = false;
            }

        }


        public Vector3 GetPosition(){
            return transform.position;
        }


        public void MoveTo(Vector3 destination)
        {
            SetDestination(destination);
            //CalculatePath(destination);
        }


        Vector3 GetTargetVector(Vector3 start, Vector3 target, float behindDistance = 0)
        {
            Vector3 targetDestination = (target.normalized * -1) * behindDistance + target;
            return (targetDestination - start);
        }


        bool SetDestination(Vector3 destination){
            currentDestination = destination;
            path = agent.path;
            hasPath = true;
            return hasPath;
        }


        NavMeshPath CalculatePath(Vector3 destination)
        {
            path = new NavMeshPath();
            NavMesh.CalculatePath(GetPosition(), destination, NavMesh.AllAreas, path);

            cornerQueue.Clear();
            foreach (Vector3 corner in path.corners){
                cornerQueue.Enqueue(corner);
            }
            GetNextCorner();

            return path;
        }


        bool GetNextCorner()
        {
            if (cornerQueue.Count > 0){
                currentDestination = cornerQueue.Dequeue();
                hasPath = true;
            }
            else{
                hasPath = false;
            }
            return hasPath;
        }


        float GetDistanceRemaining()
        {
            float distance = 0.0f;
            //Vector3[] corners = path.corners;
            Vector3[] corners = agent.path.corners;
            for (int c = 0; c < corners.Length - 1; c++){
                distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
            }
            return distance;
        }


        NavMeshPath GetPath()
        {
            var agentPath = agent.path;
            return path;
        }


        bool HasReachedDestination(){
            return GetDistanceRemaining() <= arrivalDistance;
            //return GetDistanceRemaining() <= agent.stoppingDistance && agent.pathPending == false;
        }


        void RotateTowards(Vector3 target)
        {
            Vector3 dirToLookTarget = (target - transform.forward).normalized;
            float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, angularSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
        }

	}




    public class VelocityData
    {
        float displacement;  //  S
        Vector3 initialVelocity;  //  U
        Vector3 finalVelocity;  //  V
        float acceleration;  //  A
        float time;  //  T
    }
}