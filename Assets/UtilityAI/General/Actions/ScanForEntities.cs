﻿//namespace UtilityAI
//{
//    using UnityEngine;
//    using UnityEngine.AI;

//    /// <summary>
//    /// Scans for all availible positions
//    /// </summary>
//    public class ScanForEntities : ActionBase
//    {
//        [SerializeField]
//        public float samplingRange = 20;
//        [SerializeField]
//        public float samplingDensity = 3f;

//        public override void Execute(IContext context)
//        {
//            //Debug.Log(string.Format("Executing action:  {0}", this.GetType().Name));

//            var c = (AIContext)context;

//            var player = c.entity;

//            //  Scan for enemies

//            c.enemies.Clear();

//            // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
//            var colliders = Physics.OverlapSphere(player.position, player.scanRange, Layers.players);


//            foreach (Collider col in colliders)
//            {
//                if (col.transform.CompareTag("Player"))
//                {
//                    var enemy = col.GetComponent<PlayerController>();
//                    c.enemies.Add(enemy);
//                }
//            }




//            //c.sampledPositions.Clear();

//            //var halfSamplingRange = this.samplingRange * 0.5f;
//            //var pos = player.transform.position;

//            ////  Nested loop in x and z directions.  Starting at negative half sampling range and ending at positinve half sampling range, thus sampling in a square around the entity
//            //for (var x = -halfSamplingRange; x < halfSamplingRange; x += this.samplingDensity)
//            //{
//            //    for (var z = -halfSamplingRange; z < halfSamplingRange; z += this.samplingDensity)
//            //    {
//            //        var p = new Vector3(pos.x + x, 0f, pos.z + z);

//            //        //  Sample position in the navigation mesh to ensure that the desired position is actually walkable.
//            //        NavMeshHit hit;
//            //        if (NavMesh.SamplePosition(p, out hit, this.samplingDensity * 0.5f, NavMesh.AllAreas))
//            //        {
//            //            c.sampledPositions.Add(hit.position);
//            //        }
//            //    }
//            //}

//        }


//    }
//}