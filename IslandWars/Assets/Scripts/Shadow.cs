using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider Logo)
	{
		if(Logo.tag == "Logo")
		{
			//Debug.Log("Logo Hello");
			gameObject.GetComponent<MeshRenderer>().enabled = true;

		}
	}
}
