namespace Bang
{
	using UnityEngine;

	public abstract class Entity : MonoBehaviour, IEntity
	{
		public int id { get; set; }
		
		public Vector3 position 
        {
            get{ return this.transform.position; }
            set { this.transform.position = value; }
        }


	}
}