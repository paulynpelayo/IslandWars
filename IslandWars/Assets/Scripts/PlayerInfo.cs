using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	private static PlayerInfo instance;
	public static PlayerInfo getInstance() {
		instance = FindObjectOfType(typeof(PlayerInfo)) as PlayerInfo;
		if(instance == null) {
			GameObject obj = new GameObject("PlayerInfo");
			instance = obj.AddComponent<PlayerInfo>();
		}
		
		return instance;
	}

	public int SaveBravePoint
	{
		get { return PlayerPrefs.GetInt ("BravePoints"); }
		set	{ PlayerPrefs.SetInt("BravePoints", value);	PlayerPrefs.Save(); }
	}

	public int SaveTowerHeight
	{
		get { return PlayerPrefs.GetInt ("TowerHeight"); }
		set	{ PlayerPrefs.SetInt("TowerHeight", value);	PlayerPrefs.Save(); }
	}
	
	public int SaveTowerDefense
	{
		get { return PlayerPrefs.GetInt ("TowerDefense"); }
		set	{ PlayerPrefs.SetInt("TowerDefense", value); PlayerPrefs.Save(); }
	}

	public int SaveArcherUpgrade
	{
		get { return PlayerPrefs.GetInt ("ArcherUpgrade"); }
		set	{ PlayerPrefs.SetInt("ArcherUpgrade", value); PlayerPrefs.Save(); }
	}

	public int SaveBlowGunUpgrade
	{
		get { return PlayerPrefs.GetInt ("BlowGunUpgrade"); }
		set	{ PlayerPrefs.SetInt("BlowGunUpgrade", value); PlayerPrefs.Save(); }
	}

	public int SaveSlingshotUpgrade
	{
		get { return PlayerPrefs.GetInt ("SlingshotUpgrade"); }
		set	{ PlayerPrefs.SetInt("SlingshotUpgrade", value); PlayerPrefs.Save(); }
	}

	public int SaveCannonUpgrade
	{
		get { return PlayerPrefs.GetInt ("CannonUpgrade"); }
		set	{ PlayerPrefs.SetInt("CannonUpgrade", value); PlayerPrefs.Save(); }
	}


	// Use this for initialization
	void Start () {
		Debug.Log("Instantiating");

		#if UNITY_EDITOR
			//PlayerPrefs.DeleteAll();
		#endif

		if (!PlayerPrefs.HasKey("BravePoints")) PlayerPrefs.SetInt("BravePoints", 100);
		if (!PlayerPrefs.HasKey("TowerHeight")) PlayerPrefs.SetInt("TowerHeight", 1);
		if (!PlayerPrefs.HasKey("TowerDefense")) PlayerPrefs.SetInt("TowerDefense", 1);
		if (!PlayerPrefs.HasKey("ArcherUpgrade")) PlayerPrefs.SetInt("ArcherUpgrade", 1);
		if (!PlayerPrefs.HasKey("BlowGunUpgrade")) PlayerPrefs.SetInt("BlowGunUpgrade", 1);
		if (!PlayerPrefs.HasKey("SlingshotUpgrade")) PlayerPrefs.SetInt("SlingshotUpgrade", 1);
		if (!PlayerPrefs.HasKey("CannonUpgrade")) PlayerPrefs.SetInt("CannonUpgrade", 1);

		PlayerPrefs.Save(); 

	}
	
	// Update is called once per frame
	void Update () {

	}
}
