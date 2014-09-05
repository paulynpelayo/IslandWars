using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () 
	{
		Coins = 50;
		WaveNum = 1;
		StartCoroutine(GUIManager.getInstance().PrepareForBattle());
	}
	
	// Update is called once per frame
	void Update () {


	}

	public void InitializeWave()
	{

	}

}
