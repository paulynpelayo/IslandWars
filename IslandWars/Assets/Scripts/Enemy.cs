using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int Life;
	public int Speed;
	public int Damage;
	public int Strength;
	public int CoinsGiven;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetDamage(int hit)
	{
		Life -= hit;

		if (Life <= 0)
		{
			TransformPool pool = null;
			if (transform.position.x > 0)
				pool = GameObject.Find("PoolOfEnemies_Right").GetComponent<TransformPool>();
			else if (transform.position.x < 0)
				pool = GameObject.Find("PoolOfEnemies_Left").GetComponent<TransformPool>();

			pool.returnTransform(gameObject.transform);
		}
	}

}
