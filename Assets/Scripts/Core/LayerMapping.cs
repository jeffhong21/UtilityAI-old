namespace Bang
{
	using UnityEngine;

	public class LayerMapping : MonoBehaviour 
	{

		public LayerMask coverLayer;
		public LayerMask enemiesLayer;
		public LayerMask playersLayer;


        private void Awake()
        {
            Layers.cover = coverLayer;
            Layers.enemies = enemiesLayer;
            Layers.players = playersLayer;
        }

	}
}
