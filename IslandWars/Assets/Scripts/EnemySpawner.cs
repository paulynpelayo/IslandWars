using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public TransformPool transformPool;	
	public Transform TargetPos;
	private float TimeToSpawnPerEnemy;
		
	void Start ()
	{	
		TimeToSpawnPerEnemy = 0;
	}
	
	void Update ()
	{	
		TimeToSpawnPerEnemy -= Time.deltaTime * 5;
		
		if (TimeToSpawnPerEnemy <= 0 && transformPool.NumberAvailable != 0)
		{	
			Transform Enemy = transformPool.getTransform();
			
			Enemy.position = this.transform.position;

			EnemyScript E = Enemy.GetComponent<EnemyScript>();
			E.SetPool(transformPool);
			E.SetTarget(TargetPos);
			E.enemystate = EnemyScript.Enemystate.Walking;

			TimeToSpawnPerEnemy = 5;
		}
		

	}
	
}
