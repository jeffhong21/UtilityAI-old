namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// Scans for all availible positions
    /// </summary>
    [System.Serializable]
    public class ScanForEntities : ActionBase
    {
        [SerializeField]
        public float samplingRange = 20;
        [SerializeField]
        public float samplingDensity = 3f;
        [SerializeField]
        public float scanRange = 20f;

        private string entityTag = Tags.Entity;


        protected override void Execute(IAIContext context)
        {
            //Debug.Log(string.Format("Executing action:  {0}", this.GetType().Name));

            var c = (AIContext)context;
            var entity = c.entity;


            //  Scan for hostileEntities
            c.hostileEntities.Clear();

            // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
            var colliders = Physics.OverlapSphere(entity.transform.position, scanRange, c.entitiesLayer);
            foreach (Collider col in colliders)
            {
                if (col.transform.CompareTag(entityTag)){
                    var enemy = col.GetComponent<DummyEntity>();
                    c.hostileEntities.Add(enemy.transform);
                }
            }



            //EndAction();
        }


    }
}