using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	private TrajectoryHelper Helper;
	public int Damage;
	public int Cost;
	private Transform Origin, Target;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

	/*	if (Target != null)
		{
			rigidbody.velocity = Helper.GetVelocityWithAngleAndTarget(Origin, Target.position, 45f);
			Debug.Log(rigidbody.velocity);
		}*/

		var dir = rigidbody.velocity;
		if (dir != Vector3.zero) {
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			
		}


	}

	public void setTarget(Transform origin, Transform target)
	{
		Origin = origin;
		Target = target;
	}

	public void SetHelper(TrajectoryHelper helper)
	{
		Helper = helper;
	}

	void OnTriggerEnter (Collider other)
	{

	}
}
