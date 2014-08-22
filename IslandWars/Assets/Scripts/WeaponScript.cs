using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	private TrajectoryHelper Helper;
	public int Damage;
	public int Cost;
	private Transform Origin, Target;

	void Start()
	{
		Destroy (this.gameObject, 3);
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		var dir = rigidbody.velocity;
		if (dir != Vector3.zero) {
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);			
		}

	}

	void OnTriggerEnter (Collider other)
	{	
		if (other.tag == "Target")
		{				
			EnemyScript E = other.GetComponent<EnemyScript>();
			E.GotDamage(Damage);	

			Destroy (gameObject);
			
		}
	}
}
