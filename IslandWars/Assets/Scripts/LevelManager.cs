using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LevelManager : MonoBehaviour {

	private static LevelManager instance;
	public static LevelManager getInstance()
	{
		instance = FindObjectOfType(typeof(LevelManager)) as LevelManager;
		/*if(instance == null) {
			GameObject obj = new GameObject("LevelManager");
			instance = obj.AddComponent<LevelManager>();
		}*/
			
		return instance;

	}

	private int Coins;
	public int NumOfCoins
	{
		get { return Coins; }
		set
		{
			Coins = value;
		}
	}

	private int Wave;
	public int WaveNum
	{
		get { return Wave; }
		set
		{
			Wave = value;
			InitializeWave();
		}
	}

	public bool startSailing = false, readytoSpawn = false;
	public int EnemiesKilled = 0;
	public float SpawnDelay;

	private XmlNode currentWave;
	private int numOfBrute, numOfSprinter, numOfSmasher, numOfStandard;

	private List<string> ListofEnemies = new List<string>();

	// Use this for initialization
	void Start () 
	{
		Coins = 50;
		WaveNum = 1;
		StartCoroutine(GUIManager.getInstance().PrepareForBattle());

		/*
		TextAsset textAsset = (TextAsset) Resources.Load("Levels_Info");  
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( textAsset.text );

		XmlNodeList levelsList = xmldoc.GetElementsByTagName("WaveNum"); 
	
		foreach (XmlNode levelInfo in levelsList)
		{	
			if(levelInfo.InnerText == Wave.ToString())
			{

			}
		}*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (EnemiesKilled == ListofEnemies.Count)
		{
			GUIManager.getInstance().displayVictory();
			//GUIManager.getInstance().ClickedNextLevel();
			Debug.Log("Victory");
		}

	}

	public void InitializeWave()
	{
		switch (Wave) 
		{
			case 1: 

			numOfBrute = 1;
			numOfSmasher = 1;
			numOfSprinter = 1;
			numOfStandard = 1;

			SpawnDelay = 3f;

			//Tower.getInstance().TowerLevel += 1;

			break;

			case 2: 
			
			numOfBrute = 2;
			numOfSmasher = 2;
			numOfSprinter = 2;
			numOfStandard = 1;

			SpawnDelay = 2f;

			//Tower.getInstance().TowerLevel += 1;
			
			break;

			case 3: 
			
			numOfBrute = 3;
			numOfSmasher = 3;
			numOfSprinter = 4;
			numOfStandard = 1;

			SpawnDelay = 1f;

			//Tower.getInstance().TowerLevel += 1;

			break;				
		}

		AddToList("Brute", numOfBrute);
		AddToList("Smasher", numOfSmasher);
		AddToList("Sprinter", numOfSprinter);
		AddToList("Standard", numOfStandard);

		ShuffleList();
		Debug.Log ("Initializing Wave " + WaveNum + " Completed!");
	}

	public void AddToList(string enemyName, int Count)
	{
		for (int x = 0; x < Count; x++)
			ListofEnemies.Add(enemyName);
	}

	public void ShuffleList()
	{
		int Count = ListofEnemies.Count;

		for (int x = 0; x < Count; x++)
		{
			int r = Random.Range(0,Count);

			string temp = ListofEnemies[x];
			ListofEnemies[x] = ListofEnemies[r];
			ListofEnemies[r] = temp;
		}
	}

	public List<string> GetEnemyList()
	{
		return ListofEnemies;
	}

	public void ResetLevel()
	{
		ListofEnemies.Clear();
		EnemiesKilled = 0;

		GameObject[] boats = GameObject.FindGameObjectsWithTag("SailBoat");

		foreach (GameObject boat in boats)
			boat.GetComponent<SailBoat>().ResetPosition();

		GameObject[] People = GameObject.FindGameObjectsWithTag("Player");

		//foreach (GameObject player in People)
			//Destroy (player);
	}

}
