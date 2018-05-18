namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.AI;
    using System;
    using System.Collections;
    using System.Collections.Generic;


    [RequireComponent(typeof(EntityAISteering))]
    public class EntityAIPerception : MonoBehaviour
    {
        [Header("Perception")]
        [Range(0, 360)]
        public float fieldOfView = 108f;
        public float sightRange = 20;
        public float yOffset = 1;

        [Header("Scan Interval")]
        public float scanInterval = 0.2f;
        private float _lastScan;

        public LayerMask obstaclesLayer;

        public List<GameObject> units;

        Color sightRangeColor = new Color(1, 1, 1, 0.5f);
        Color viewAngleColor = new Color(1, 1, 1, 0.15f);


		private void OnEnable()
		{
            units = new List<GameObject>();
		}

		void Update()
		{
            var time = Time.time;
            if (time - _lastScan < this.scanInterval)
            {
                return;
            }
            _lastScan = time;

		}


		public List<GameObject> FindNeighbors(float scanRadius)
        {
            var colliders = Physics.OverlapSphere(this.transform.position, scanRadius, Layers.entites);


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

                units.Add(coll.gameObject);
            }


            return units;
        }


        //  Calculates if npc can see target.
        public bool CanSeeTarget(Transform target)
        {
            var targetPosition = new Vector3(target.position.x, (target.position.y + transform.position.y), target.position.z);
            var dirToPlayer = (target.position - transform.position).normalized;

            var angleBetweenNpcAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            if (Vector3.Distance(transform.position, targetPosition) < sightRange &&
                angleBetweenNpcAndPlayer < fieldOfView / 2f &&
                Physics.Linecast(transform.position, target.position, obstaclesLayer) == false)
            {
                return true;
            }
            return false;
        }


        Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal = false)
        {
            if (angleIsGlobal == false){
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }



        void OnDrawGizmos()
        {
            //  View Arch
            //Handles.color = entity.hasLocationOfInterest ? targetInViewRangeColor : viewAngleColor;
            Handles.color = viewAngleColor;
            Handles.DrawSolidArc(transform.position + Vector3.up * yOffset, Vector3.up, DirFromAngle(transform.rotation.y), fieldOfView / 2, sightRange);
            Handles.DrawSolidArc(transform.position + Vector3.up * yOffset, Vector3.up, DirFromAngle(transform.rotation.y), -fieldOfView / 2, sightRange);

            //  Sight Range
            Handles.color = sightRangeColor;
            Handles.DrawWireArc(transform.position + Vector3.up * yOffset, Vector3.up, Vector3.forward, 360, sightRange);
        }

    }

}