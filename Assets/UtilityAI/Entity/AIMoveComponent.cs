namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class AIMoveComponent : MonoBehaviour
    { 

        public float speed = 8;    
        public float maxSpeed = 12; 
        public float acceleration = 8; 
        public float maxAcceleration = 24; 

        public float angularSpeed = 135;                // "How fast the entity turns."

        public float stoppingDistance = 1f;             // "The range for which the entity should slow down when moving or not steerCmpnt if stationary."
        public float radius = 0.5f;                     // "How wide the entity is."
        public float separationDistance = 0.5f;         // "The separation distance - i.e. the desired distance between the extents of any two units"



        private NavMeshAgent agent;
        public NavMeshPath path;
        public bool hasPath;


        private Queue<Vector3> cornerQueue;
        private Vector3 previousDestination;
        private Vector3 currentDestination;
        private float scanInterval = 0.5f;
        private float nextInterval;


        [Header("----- Debug -----")]
        public bool debugInfo;





		void Awake()
		{
            //if (entity == null) entity = GetComponent<EntityAIController>();
            if (agent == null) agent = GetComponent<NavMeshAgent>();

		}

		void OnEnable()
		{
            agent.speed = speed;
            agent.acceleration = acceleration;
            agent.angularSpeed = angularSpeed;
            agent.radius = radius;
            agent.stoppingDistance = Mathf.Abs(agent.stoppingDistance) < float.Epsilon ? stoppingDistance : agent.stoppingDistance;
            agent.autoBraking = false;
            // we give the unit a random avoidance priority so as to ensure that units will actually avoid each other (since same priority units will not try to avoid each other)
            agent.avoidancePriority = UnityEngine.Random.Range(0, 99);


            path = new NavMeshPath();
            cornerQueue = new Queue<Vector3>();
		}

		private void OnDisable()
		{
            path = null;
            cornerQueue = null;
		}


        #region Update

        void Update()
        {
          //if(isMoving && agent.isStopped == false)
          //{
          //    Vector3 dirToTarget = agent.steeringTarget - this.transform.position;
          //    float turnAngle = Vector3.Angle(this.transform.forward, dirToTarget);
          //    float currentAcceleration = Mathf.Clamp((turnAngle * agent.speed), acceleration, maxAcceleration);
          //    agent.acceleration = currentAcceleration;
          //    //agent.acceleration = (turnAngle * agent.speed);
          //}


          if(hasPath)
          {
              var distRemaining = (currentDestination - this.transform.position).sqrMagnitude;
              if(distRemaining > stoppingDistance)
              {
                  RotateTowards(currentDestination);

                  float distSpeedModifier = GetSpeedModifier(distRemaining);
                  Vector3 velocity = this.transform.forward * Time.deltaTime * distSpeedModifier;
                  velocity.y = 0f;

                  agent.Move(velocity);

                  if(Time.time > nextInterval){
                      CalculatePath(currentDestination);
                      nextInterval = Time.time + scanInterval;
                  }

              }
              else{
                  GetNextCorner();
              }
          }


          if(HasReachedDestination()){
              StopWalking();
          }
        }

        #endregion

        #region FixedUpdate

  //      private void FixedUpdate()
		//{
  //          if(hasPath)
  //          {
  //              var distRemaining = (currentDestination - this.transform.position).sqrMagnitude;
  //              if (distRemaining > stoppingDistance)
  //              {
  //                  //Vector3 steering = Vector3.zero;
  //                  //Vector3 followForce = GetTargetVector(this.GetPosition(), this.GetDestination());
  //                  //steering += followForce;
  //                  ////Vector3 separationForce = GetSeparationVector(separationDistance, 20f);
  //                  ////steering += separationForce;

  //                  RotateTowards(currentDestination);

  //                  Vector3 velocity = this.transform.forward * Time.fixedDeltaTime * acceleration;
  //                  velocity.y = 0f;
  //                  agent.Move(velocity);
  //              }
  //              else{
  //                  GetNextCorner();
  //              }

  //          }
		//}

        #endregion

		public void MoveTo(Vector3 destination)
        {
            //KeepWalking();
            //agent.SetDestination(destination);

            path = CalculatePath(destination);
            foreach(Vector3 corner in path.corners){
                cornerQueue.Enqueue(corner);
            }
            GetNextCorner();
        }


        public Vector3 GetPosition(){
            return transform.position;
        }

        public Vector3 GetDestination(){
            return currentDestination;
        }

        public void SetDestination(Vector3 destination){
            previousDestination = currentDestination;
            currentDestination = destination;
        }

        public Vector3 GetTargetVector(Vector3 start, Vector3 target, float behindDistance = 0){
            Vector3 targetDestination = (target.normalized * -1) * behindDistance + target;
            return (targetDestination - start);
        }

        //public Vector3 GetVelocity(bool clampVelocity = true){
        //    var time = Time.deltaTime;
        //    var velocity = (currentDestination - previousDestination) / time;
        //    if(clampVelocity) velocity = Vector3.ClampMagnitude(velocity, this.maxSpeed);

        //    return agent.velocity;
        //}

        public double GetMaxVelocity(){
            return float.PositiveInfinity;
        }

        NavMeshPath CalculatePath(Vector3 destination)
        {
            path = new NavMeshPath();
            NavMesh.CalculatePath(GetPosition(), destination, NavMesh.AllAreas, path);
            return path;
        }

        /// <summary>
        /// Get next corner of the NavMeshPath
        /// </summary>
        /// <returns><c>true</c>, if next corner was gotten, <c>false</c> otherwise.</returns>
        bool GetNextCorner()
        {
            if(cornerQueue.Count > 0){
                currentDestination = cornerQueue.Dequeue();
                hasPath = true;
            }
            else{
                hasPath = false;
            }
            return hasPath;
        }


        void RotateTowards(Vector3 target)
        {
            Vector3 dirToLookTarget = (target - transform.position).normalized;
            float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, angularSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;

            //Vector3 targetDir = target - transform.position;
            //float _acceleration = acceleration * Time.deltaTime;
            //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, _acceleration, 0.0f);
            //transform.rotation = Quaternion.LookRotation(newDir);

            //Vector3 dirToTarget = target - this.transform.position;
            //Quaternion fromToRotation = Quaternion.LookRotation(dirToTarget);
            //Quaternion rotation = Quaternion.SlerpUnclamped(this.transform.rotation, fromToRotation, Time.deltaTime);
            //this.transform.rotation = rotation;
        }

        float GetSpeedModifier(float distance)
        {
            if (distance <= stoppingDistance){
                float speedDampen = (stoppingDistance - distance) / stoppingDistance;
                return (1 - speedDampen) * speed;
            }
            else{
                return speed;
            }
        }


        public List<GameObject> FindNeighbors(float scanRadius)
        {
            var colliders = Physics.OverlapSphere(this.transform.position, scanRadius, Layers.entites);
            List<GameObject> neighbor = new List<GameObject>();

            for (int i = 0; i < colliders.Length; i++)
            {
                var coll = colliders[i];
                if (coll == null)
                {
                    // ignore null entries
                    continue;
                }

                if (ReferenceEquals(coll.gameObject, this.gameObject))
                {
                    // Do not record 'self'
                    continue;
                }

                neighbor.Add(coll.gameObject);
            }


            return neighbor;
        }


        public Vector3 GetSeparationVector(float maxSeparation, float scanRadius)
        {
            Vector3 pos = this.GetPosition();
            List<GameObject> neighbor = FindNeighbors(scanRadius);
            Vector3 force = Vector3.zero;
            foreach (GameObject go in neighbor)
            {
                force += (go.transform.position - pos);
            }
            // equipare the force
            force /= neighbor.Count;
            // scale it
            force *= maxSeparation;
            //return the inverse
            return force * -1;
        }





        IEnumerator LookTowards()
        {
            Vector3 dirToLookTarget;
            float targetAngle;
            Vector3[] corners = agent.path.corners;
            while(agent.path.GetCornersNonAlloc(agent.path.corners) > 0)
            {
                int index = agent.path.GetCornersNonAlloc(agent.path.corners) - 2;
                if (index < 0)
                    break;
                
                dirToLookTarget = (corners[index] - transform.position).normalized;
                targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

                while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f) //  Mathf.DeltaAngle tells us the difference between two angles.
                {
                    float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, angularSpeed * Time.deltaTime);
                    transform.eulerAngles = Vector3.up * angle;
                }
                agent.acceleration = acceleration;
                yield return null;
            }
        }



        public bool HasReachedDestination(){
            return GetDistanceRemaining() <= stoppingDistance;
            //return GetDistanceRemaining() <= agent.stoppingDistance && agent.pathPending == false;
        }

        /// <summary>
        /// Get the remaining distance from NavMeshAgent.  Better than navMeshAgent.remainingDistance
        /// </summary>
        /// <returns>The distance remaining.</returns>
        public float GetDistanceRemaining()
        {
            float distance = 0.0f;
            Vector3[] corners = path.corners;
            //Vector3[] corners = agent.path.corners;
            for (int c = 0; c < corners.Length - 1; c++)
            {
                distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
            }
            return distance;
        }


        //  Get a random location within range.
        public bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            NavMeshHit navHit;
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;

            if (NavMesh.SamplePosition(randomPoint, out navHit, range, NavMesh.AllAreas)){
                result = navHit.position;
                return true;
            }
            else{
                result = center;
                return false;
            }
        }


        public void KeepWalking(){
            agent.isStopped = false;
        }

        public void StopWalking()
        {
            agent.isStopped = true;
            agent.ResetPath();
        }







		private void OnGUI()
		{

            if(debugInfo)
            {
                var cam = Camera.main;
                if (cam == null)
                    return;

                Vector2 size = Vector2.zero;
                int index = 1;

                var agentHasPathContent = new GUIContent("Is Agent Moving:  " + agent.hasPath.ToString());
                size = new GUIStyle(GUI.skin.label).CalcSize(agentHasPathContent);
                GUI.Label(new Rect(5f, 5f + (size.y * index), size.x, size.y), agentHasPathContent);
                index++;

                var isMovingContent = new GUIContent("Has Reached Destination:  " + HasReachedDestination().ToString());
                size = new GUIStyle(GUI.skin.label).CalcSize(isMovingContent);
                GUI.Label(new Rect(5f, 5f + (size.y * index), size.x, size.y), isMovingContent);
                index++;

                var getRemainingDistanceContent = new GUIContent("Remaining Distance:  " + GetDistanceRemaining().ToString());
                size = new GUIStyle(GUI.skin.label).CalcSize(getRemainingDistanceContent);
                GUI.Label(new Rect(5f, 5f + (size.y * index), size.x, size.y), getRemainingDistanceContent);
                index++;

                var agentAccelerationContent = new GUIContent("Acceleration:  " + agent.acceleration.ToString());
                size = new GUIStyle(GUI.skin.label).CalcSize(agentAccelerationContent);
                GUI.Label(new Rect(5f, 5f + (size.y * index), size.x, size.y), agentAccelerationContent);
                index++;

                var speedContent = new GUIContent("Speed:  " + agent.speed.ToString());
                size = new GUIStyle(GUI.skin.label).CalcSize(speedContent);
                GUI.Label(new Rect(5f, 5f + (size.y * index), size.x, size.y), speedContent);
                index++;

                if(path != null){
                    var turnAngleContent = new GUIContent("Number Of Corners:  " + path.GetCornersNonAlloc(path.corners).ToString());
                    size = new GUIStyle(GUI.skin.label).CalcSize(turnAngleContent);
                    GUI.Label(new Rect(5f, 5f + (size.y * index), size.x, size.y), turnAngleContent);
                    index++;
                }

            }

		}



    }
}