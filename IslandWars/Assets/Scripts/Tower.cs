using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	private static Tower instance;
	public static Tower getInstance() {
		instance = FindObjectOfType(typeof(Tower)) as Tower;
		if(instance == null) {
			GameObject obj = new GameObject("Tower");
			instance = obj.AddComponent<Tower>();
		}
		return instance;
	}

	private int TowerLvl = 0;
	public int TowerLevel
	{
		get { return TowerLvl; }
		set
		{
			TowerLvl = value;
			IncreaseLevel();
		}

	}

	public Transform TowerRoof;
	public Transform[] TowerPrefabs;

	private int Life = 100;

	// Use this for initialization
	void Start () {
		TowerLvl = PlayerInfo.getInstance().SaveTowerHeight;

		for (int x = 1; x < TowerLvl; x++)
			IncreaseLevel();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setDamage (int damage)
	{
		Life -= damage; 

		if (Life  > 0)
			GUIManager.getInstance().setLifeBar(damage);
		else GUIManager.getInstance().displayGameOver();
	}

	private void IncreaseLevel()
	{
		float offset = 0.45f;

		TowerRoof.position = new Vector2(TowerRoof.position.x, TowerRoof.position.y + offset);

		for (int x = 0; x < TowerPrefabs.Length; x++)
		{
			TowerPrefabs[x].position = new Vector2(TowerPrefabs[x].position.x, TowerPrefabs[x].position.y + offset);;

			if (x < TowerLvl - 1)
			{
				TowerPrefabs[x].gameObject.active = true;
			}
		}
	}
}
