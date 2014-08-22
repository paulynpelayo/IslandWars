﻿using UnityEngine;
using System.Collections;

public class TargetList : MonoBehaviour {

	//public GameObject RangeCollider;
	private ArrayList ListOfEnemies;
	private Vector2 Origin =  new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		ListOfEnemies = new ArrayList();
	}

	public GameObject getNearestEnemy()
	{	
		if (ListOfEnemies.Count != 0)
		{
			GameObject Nearest = null;
			float ShortestDistance = 0, Distance = 0;
					
			foreach (GameObject enemy in ListOfEnemies)
			{
				Distance = Vector2.Distance(Origin, enemy.transform.position);

				if (Nearest == null || Distance < ShortestDistance)
				{
					ShortestDistance = Distance;
					Nearest = enemy;
				}				
			}

			return Nearest;
		}
		else return null;
	}

	public void AddToList(GameObject Enemy)
	{
		if (!ListOfEnemies.Contains(Enemy))
			ListOfEnemies.Add (Enemy);

	}

	public void RemoveFromList(GameObject DeadEnemy)
	{
		ListOfEnemies.Remove (DeadEnemy);
	}

}
