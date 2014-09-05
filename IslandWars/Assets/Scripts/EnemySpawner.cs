using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public TransformPool transformPool;	
	public Transform TargetPos;
	public bool isRightSpawner;
	private float TimeToSpawnPerEnemy;

	void Start ()
	{	
		TimeToSpawnPerEnemy = 0;
	}
	
	void Update ()
	{	
		if (LevelManager.getInstance().readytoSpawn)
		{
			TimeToSpawnPerEnemy -= Time.deltaTime * 10;
			
			if (TimeToSpawnPerEnemy <= 0 && transformPool.NumberAvailable != 0)
			{	
				Transform Enemy = transformPool.getTransform();
				
				Enemy.position = this.transform.position;

				if (isRightSpawner)
					Enemy.localScale = new Vector2(-0.5f, 0.5f);

				EnemyScript E = Enemy.GetComponent<EnemyScript>();
				E.SetPool(transformPool);
				E.SetTarget(TargetPos);
				E.enemystate = EnemyScript.Enemystate.Walking;

				TimeToSpawnPerEnemy = 10;
			}
		}	

	}
	
}
