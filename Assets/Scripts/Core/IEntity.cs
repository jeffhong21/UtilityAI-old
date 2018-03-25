namespace Bang
{
	using UnityEngine;

	public interface IEntity
	{
		int id { get; set; }
		Vector3 position { get; set; }
		GameObject gameObject { get;  }

	}
}