using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	
	public Transform[] RightSpawners, LeftSpawners;
	
	private bool isRightSpawner;
	private float TimeToSpawnPerEnemy;
	private int TotalEnemy;
	private bool CanSpawn = false;
	private Transform TargetPos;
	private TransformPool BrutePool, SmasherPool, SprinterPool, StandardPool;	
	
	private List<string> ListofEnemies = new List<string>();
	
	void Start()
	{
		BrutePool = GameObject.Find ("Pool_Of_Brute").GetComponent<TransformPool>();
		SmasherPool = GameObject.Find ("Pool_Of_Smasher").GetComponent<TransformPool>();
		SprinterPool = GameObject.Find ("Pool_Of_Sprinter").GetComponent<TransformPool>();
		StandardPool = GameObject.Find ("Pool_Of_Standard").GetComponent<TransformPool>();
		
	}
	
	void Update ()
	{	
		if (LevelManager.getInstance().startSailing)
		{
			TimeToSpawnPerEnemy = LevelManager.getInstance().SpawnDelay;
			ListofEnemies = LevelManager.getInstance().GetEnemyList();
			TotalEnemy = ListofEnemies.Count;
		}
		
		if (LevelManager.getInstance().readytoSpawn)
		{
			CanSpawn = true;
			LevelManager.getInstance().startSailing = false;
			LevelManager.getInstance().readytoSpawn = false;
		}
		
		if (CanSpawn) 
		{		
			CanSpawn = false;
			
			int numPerSpawn = Random.Range(0, 4);
			
			for (int x = 0; x < numPerSpawn; x++)
			{
				if (TotalEnemy > 0)
				{
					TotalEnemy--;
					
					TransformPool CurrentPool = null;
					
					switch (ListofEnemies[TotalEnemy])
					{
					case "Brute": CurrentPool = BrutePool; break;
					case "Smasher": CurrentPool = SmasherPool; break;
					case "Sprinter": CurrentPool = SprinterPool; break;
					case "Standard": CurrentPool = StandardPool; break;
					}
					
					Transform Enemy = CurrentPool.getTransform();
					
					int chosenSpawner;
					
					if (isRightSpawner)
					{
						isRightSpawner = false;
						chosenSpawner = Random.Range(0, RightSpawners.Length);
						Enemy.position = RightSpawners[chosenSpawner].position;
						Enemy.localScale = new Vector2(-0.5f, 0.5f);
						TargetPos = RightSpawners[chosenSpawner].GetChild(0);
					}
					else 
					{
						isRightSpawner = true;
						chosenSpawner = Random.Range(0, LeftSpawners.Length);					
						Enemy.position = LeftSpawners[chosenSpawner].position;
						Enemy.localScale = new Vector2(0.5f, 0.5f);
						TargetPos = LeftSpawners[chosenSpawner].GetChild(0);
					}
					
					EnemyScript E = Enemy.GetComponent<EnemyScript>();
					E.SetPool(CurrentPool);
					E.SetTarget(TargetPos);
					E.enemystate = EnemyScript.Enemystate.Walking;
				}
			}
			StartCoroutine(CountDown());			
			
			
		}
		//else StartCoroutine(CountDown());
		
	}
	
	IEnumerator CountDown()
	{
		Time.timeScale = 1;
		
		yield return new WaitForSeconds(TimeToSpawnPerEnemy);
		
		CanSpawn = true;
	}	
	
}
