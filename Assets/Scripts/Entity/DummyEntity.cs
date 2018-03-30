using UnityEngine;
using System.Collections;
using Bang;

public class DummyEntity : Entity
{
    public Vector2 wanderTimeRange = new Vector2(2f, 6f);
    public float movementSpeed = 1f;
    [SerializeField]
    float wanderTime;

	private void Awake()
	{
        this.tag = "Entity";
	}

	void Start(){

	}


	void Update()
	{
        if(wanderTime > 0){
            transform.Translate(Vector3.forward * movementSpeed);
            wanderTime -= Time.deltaTime;
        }
        else{
            wanderTime = UnityEngine.Random.Range(wanderTimeRange.x, wanderTimeRange.y);
            Wander();
        }
	}

    void Wander(){
        transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
    }
}
