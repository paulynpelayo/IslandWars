using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformPool : MonoBehaviour {

	//public Transform transformprefab;
	//public int initialAvailability; 

	public Transform EnemyPrefab;

	private List<Transform> EnemyPool = new List<Transform>();

	public int initialAvailability = 10;

	//private Transform[] pool;
	//public Transform pool0;
	//public Transform pool1;
	//public Transform pool2;
	//public Transform pool3;
	////public Transform pool4;
	//public Transform pool5;
	private int NumberAvailable;	
	
	void Start ()
	{
		/*pool = new Transform[initialAvailability];
		
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
		*/
		NumberAvailable = initialAvailability;

		for (int i = 0; i < initialAvailability; i++)
		{
			EnemyPool.Add(Instantiate(EnemyPrefab) as Transform);
			Deactivate( EnemyPool[i] );

		}

		Debug.Log ("Initalizing " + EnemyPrefab.name + " Pool Done!");

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
		//foreach (Transform child in transform)
			//Activate(child);
		
		transform.gameObject.active = true;
		if (transform.renderer != null)
			transform.renderer.enabled = true;
	}
	
	public Transform getTransform()
	{	
		if (NumberAvailable > 0)
		{
			NumberAvailable--;

			Activate( EnemyPool[NumberAvailable] );
			return EnemyPool[NumberAvailable];
		}
		else
		{
			Transform New = Instantiate(EnemyPrefab) as Transform;
			EnemyPool.Add(New);
			return New;
		}

		Debug.Log("Not enough " + EnemyPrefab + "s in " + EnemyPool);
		return null;
	}
	
	public bool returnTransform(Transform transform)
	{
	
		if (NumberAvailable < EnemyPool.Count)
		{
			Deactivate(transform);
			EnemyPool[NumberAvailable] = transform;
			NumberAvailable++;
			return true;
		}

		Debug.Log("available" + NumberAvailable);
		Debug.Log(transform + " could not be returned to " + EnemyPool.Count + " because it is full");
		return false;
	}

	public int getNumberAvailable
	{
		get
		{
			return NumberAvailable;
		}
	}
}
