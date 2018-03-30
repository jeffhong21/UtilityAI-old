using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NpcBehavior;
using Bang;


public class NPC_TakeDamage : MonoBehaviour, IDamageable
{
	NPC_BehaviorAI npc;
	public ParticleSystem deathVfx;


	void Awake(){
		npc = GetComponent<NPC_BehaviorAI>();
	}

	void Start () {
	}
	

	public void TakeHit(int damage, Vector3 hitPoint, Vector3 hitDirection){
		// Debug.Log("Take Hit");
		npc.health -= damage;
		if(npc.health <= 0 && !npc.isDead)
		{
			GameObject.Destroy( Instantiate(deathVfx, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection) ) , deathVfx.main.duration);
			Die();
		}
	}


	void Die(){
		npc.isDead = true;
		RemoveThis();

		GameObject.Destroy (gameObject);
	}


	void RemoveThis()
	{
		if(GetComponent<Collider>()!=null){
			Destroy(GetComponent<Collider>());
		}
		if(GetComponent<Rigidbody>()!=null){
			Destroy(GetComponent<Rigidbody>());
		}
		gameObject.layer = LayerMask.NameToLayer("Default");  //  So AI doesn't keep detecting.
		Destroy(this);
	}
}

