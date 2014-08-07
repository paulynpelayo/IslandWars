using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public TransformPool transformPool;	
	private float TimeToSpawnPerEnemy;
	
	//private Vector3[] SpawnLocation = new Vector3[3];
	
	void Start ()
	{	
		TimeToSpawnPerEnemy = 0;
	}
	
	void Update ()
	{	
		TimeToSpawnPerEnemy -= Time.deltaTime * 5;
		
		if (TimeToSpawnPerEnemy <= 0)
		{
			var Enemy = transformPool.getTransform();
			
			Enemy.position = this.transform.position;
			
			TimeToSpawnPerEnemy = 5;
		}
		
		
	}
	
}
