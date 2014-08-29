using UnityEngine;
using System.Collections;

public class DotMovement : MonoBehaviour {

	public Transform dot;
	int i = -1;
	// Use this for initialization
	void Start () {
		InvokeRepeating("Shift",0,1);
	
	}
	
	void Shift()
	{
		Debug.Log(i);

		dot.position =dot.parent.TransformPoint(i,0,0);
		if(i==1)
		{
			i = -1;
		}
		else
			i++;
	}
}
