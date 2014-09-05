using UnityEngine;
using System.Collections;

public class StoreManager : MonoBehaviour {

	private static StoreManager instance;
	public static StoreManager getInstance()
	{
		instance = FindObjectOfType(typeof(StoreManager)) as StoreManager;
		if(instance == null) {
			GameObject obj = new GameObject("StoreManager");
			instance = obj.AddComponent<StoreManager>();
		}
		
		return instance;
		
	}

	public Transform[] People;
	public Transform SpawnPoint;

	private int CoinsAvailable;

	// Use this for initialization
	void Start () {

		CoinsAvailable = LevelManager.getInstance().NumOfCoins;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Buying (int item)
	{
		//if (CoinsAvailable >= Cost[item])
		//{
			int Cost =  People[item].GetComponent<PeopleScript>().Cost;
			LevelManager.getInstance().NumOfCoins -= Cost;
			//Debug.Log("Item " + People[item].gameObject.name + " bought");
			//Debug.Log("number of Coins remaining: " + LevelManager.getInstance().NumOfCoins);

			Transform SpawnedPeople = Instantiate(People[item]) as Transform;	
			SpawnedPeople.position = SpawnPoint.position;

		//}
		//else Debug.Log ("Insufficent Funds");
	}

	public bool[] AvailableItems()
	{
		CoinsAvailable = LevelManager.getInstance().NumOfCoins;
		bool[] isAvailable = new bool[People.Length];

		for (int x = 0; x < People.Length; x++)
		{
			if (CoinsAvailable >= People[x].GetComponent<PeopleScript>().Cost)
			    isAvailable[x] = true;
			else isAvailable[x] = false;
		}

		return isAvailable;
	}
}
