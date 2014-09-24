using UnityEngine;
using System.Collections;

public class EndCreditScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// Raises the collision enter2 d event.
	/// </summary>
	/// <param name="boxcoll">Boxcoll.</param>
	void OnCollisionEnter2D(Collision2D boxcoll) 
	{
		if (boxcoll.gameObject.tag == "collide")
		{
			//boxcoll.gameObject.SendMessage("ApplyDamage", 10);
			Debug.Log("EXIT CREDIT SCENE");
		}
		
	}
}
