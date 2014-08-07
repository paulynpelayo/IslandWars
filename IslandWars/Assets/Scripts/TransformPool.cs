using UnityEngine;
using System.Collections;

public class TransformPool : MonoBehaviour {
	
	//public Transform transformprefab;
	public int initialAvailability; 
	
	private Transform[] pool;
	public Transform pool0;
	public Transform pool1;
	public Transform pool2;
	public Transform pool3;
	public Transform pool4;
	public Transform pool5;
	private int numberAvailable;	
	
	void Start ()
	{
		pool = new Transform[initialAvailability];
		
		pool[0] = Instantiate(pool0) as Transform;
		pool[1] = Instantiate(pool1) as Transform;
		pool[2] = Instantiate(pool2) as Transform;
		pool[3] = Instantiate(pool3) as Transform;
		pool[4] = Instantiate(pool4) as Transform;
		pool[5] = Instantiate(pool5) as Transform;
		
		for (int i = 0; i < initialAvailability; i++)
		{
			//pool[i] = Instantiate(transformprefab) as Transform;	
			Deactivate( pool[i] );
		}
		numberAvailable = initialAvailability;
	}
	
	public void Deactivate(Transform transform)
	{
		foreach (Transform child in transform)
			Deactivate(child);
		
		transform.gameObject.active = false;
		if (transform.renderer != null)
			transform.renderer.enabled = false;
	}
	
	private void Activate(Transform transform)
	{
		foreach (Transform child in transform)
			Activate(child);
		
		transform.gameObject.active = true;
		if (transform.renderer != null)
			transform.renderer.enabled = true;
	}
	
	public Transform getTransform()
	{	
		if (numberAvailable > 0)
		{
			numberAvailable--;
			Activate( pool[numberAvailable] );
			return pool[numberAvailable];
		}
		//Debug.Log("Not enough " + transformprefab + "s in " + pool);
		return null;
	}
	
	public bool returnTransform(Transform transform)
	{
		if (numberAvailable < pool.Length)
		{
			Deactivate(transform);
			pool[numberAvailable] = transform;
			numberAvailable++;
			return true;
		}
		Debug.Log("available" + numberAvailable);
		Debug.Log(transform + " could not be returned to " + pool.Length + " because it is full");
		return false;
	}
}
