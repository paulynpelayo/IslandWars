using UnityEngine;
using System.Collections;

public class TargetList : MonoBehaviour {

	private ArrayList ListOfEnemies;
	private Vector2 Origin =  new Vector2(0, 0);

	// Use this for initialization
	void Awake () {
		ListOfEnemies = new ArrayList();
	}

	public GameObject GetEnemy(string AttackTarget)
	{
		switch (AttackTarget)
		{
			case "Nearest": return getNearestEnemy(); break;
			case "Farthest": return getFarthestEnemy(); break;
			case "Weakest": return getWeakestEnemy(); break;
			case "Strongest": return getStrongestEnemy(); break;
		}

		return null;
	}

	private GameObject getNearestEnemy()
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

	private GameObject getFarthestEnemy()
	{	
		if (ListOfEnemies.Count != 0)
		{
			GameObject Farthest = null;

			Farthest = (GameObject)ListOfEnemies[ListOfEnemies.Count - 1];

			return Farthest;
		}
		else return null;
	}

	private GameObject getWeakestEnemy()
	{	
		if (ListOfEnemies.Count != 0)
		{
			GameObject Weakest = null;
			int WeakestStrength = 50, Strength = 0;
			
			foreach (GameObject enemy in ListOfEnemies)
			{
				if (enemy.GetComponent<EnemyScript>().enemystate != EnemyScript.Enemystate.Dying)
				{
					Strength = enemy.GetComponent<EnemyScript>().Damage;
					
					if (Weakest == null || Strength < WeakestStrength)
					{
						WeakestStrength = Strength;
						Weakest = enemy;
					}
				}
			}
			
			return Weakest;
		}
		else return null;
	}

	private GameObject getStrongestEnemy()
	{	
		if (ListOfEnemies.Count != 0)
		{
			GameObject Strongest = null;
			int StrongestStrength = 0, Strength = 0;
			
			foreach (GameObject enemy in ListOfEnemies)
			{
				if (enemy.GetComponent<EnemyScript>().enemystate != EnemyScript.Enemystate.Dying)
				{
					Strength = enemy.GetComponent<EnemyScript>().Damage;
					
					if (Strongest == null || Strength > StrongestStrength)
					{
						StrongestStrength = Strength;
						Strongest = enemy;
					}
				}
			}
			
			return Strongest;
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
