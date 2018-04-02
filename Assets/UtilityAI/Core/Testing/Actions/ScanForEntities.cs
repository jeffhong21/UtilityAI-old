namespace UtilityAI
{
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// Scans for all availible positions
    /// </summary>
    public class ScanForEntities : ActionBase
    {
        [SerializeField]
        public float samplingRange = 20;
        [SerializeField]
        public float samplingDensity = 3f;
        public float scanRange = 20f;

        private string entityTag = Bang.Tags.Entity;

        protected override void Execute(IContext context)
        {
            //Debug.Log(string.Format("Executing action:  {0}", this.GetType().Name));

            var c = (AIContext)context;
            var entity = c.entity;


            //  Scan for enemies
            c.enemies.Clear();

            // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
            var colliders = Physics.OverlapSphere(entity.position, scanRange, c.entitiesLayer);



            foreach (Collider col in colliders)
            {
                if (col.transform.CompareTag(entityTag)){
                    var enemy = col.GetComponent<DummyEntity>();
                    c.enemies.Add(enemy.transform);
                }
            }



            EndAction();
        }


    }
}