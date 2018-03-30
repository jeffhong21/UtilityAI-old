namespace Bang
{
	using UnityEngine;

	public class LayerMapping : MonoBehaviour 
	{

		public LayerMask entitesLayer;
		public LayerMask playersLayer;


        private void Awake()
        {
            Layers.entites = entitesLayer;
            Layers.players = playersLayer;
        }

	}
}
