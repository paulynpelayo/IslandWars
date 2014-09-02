using UnityEngine;
using System.Collections;

public class TargetList : MonoBehaviour {

	private ArrayList ListOfEnemies;
	private Vector2 Origin =  new Vector2(0, 0);

	// Use this for initialization
	void Awake () {
		ListOfEnemies = new ArrayList();
	}

	public GameObject getNearestEnemy()
	{	
		if (ListOfEnemies.Count != 0)
		{
			GameObject Nearest = null;
			float ShortestDistance = 50, Distance = 0;
					
			foreach (GameObject enemy in ListOfEnemies)
			{
				if (enemy.GetComponent<EnemyScript>().enemystate != EnemyScript.Enemystate.Dying)
				{
					Distance = Vector2.Distance(Origin, enemy.transform.position);

					if (Nearest == null || Distance < ShortestDistance)
					{
						ShortestDistance = Distance;
						Nearest = enemy;
					}
				}
			}
		
			return Nearest;
		}
		else return null;
	}
	/*
	public void AddToList(GameObject Enemy)
	{
		if (!ListOfEnemies.Contains(Enemy))
			ListOfEnemies.Add (Enemy);
		Debug.Log (ListOfEnemies.Count);
	}*/

	public void RemoveFromList(GameObject DeadEnemy)
	{
		ListOfEnemies.Remove (DeadEnemy);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Target")
			if (!ListOfEnemies.Contains(other.gameObject))
				ListOfEnemies.Add (other.gameObject);						
	}
}
