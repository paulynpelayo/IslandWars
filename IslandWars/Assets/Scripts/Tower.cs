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
	
	private int TowerDef = 0;
	public int TowerDefense
	{
		get { return TowerDef; }
		set
		{
			TowerDef = value;
			IncreaseLife();
		}
	}

	public tk2dSprite TowerFront, TowerBack;
	public Transform TowerRoof;
	public Transform[] TowerPrefabs;
	
	public tk2dSlicedSprite LifeBase;
	public tk2dClippedSprite LifeBar;

	public int MaxLife = 100;
	private int Life = 100;

	public AudioClip DestroySound;
	
	// Use this for initialization
	void Start () {
		TowerLvl = PlayerInfo.getInstance().SaveTowerHeight;
		TowerDef = PlayerInfo.getInstance().SaveTowerDefense;
		
		for (int x = 1; x < TowerLvl; x++)
			IncreaseLevel();

		for (int x = 1; x < TowerDef; x++)
			IncreaseLife();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void setDamage (int damage)
	{
		Life -= damage; 

		audio.PlayOneShot(DestroySound);
		
		if (Life  > 0)
			GUIManager.getInstance().setLifeBar(damage);
		else GUIManager.getInstance().displayGameOver();
	}
	
	private void IncreaseLevel()
	{
		float offset = 0.55f;
		
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
	
	private void IncreaseLife()
	{
		Vector2 Dimension = LifeBase.dimensions;
		LifeBase.dimensions = new Vector2 (Dimension.x + 50, Dimension.y);
		
		Vector2 Scale = new Vector2(LifeBar.scale.x + 0.15f, LifeBar.scale.y);
		LifeBar.scale = Scale;

		MaxLife += 25;
		Life += 25;

		int spriteNum = 0;

		switch (TowerDef)
		{
			case 3:				
				
				spriteNum = TowerFront.GetSpriteIdByName ("wallfrontSTONE");
				TowerFront.SetSprite(spriteNum);
				spriteNum = TowerBack.GetSpriteIdByName ("wallbackSTONE");
				TowerBack.SetSprite(spriteNum);

			break;

			case 4:

				spriteNum = TowerFront.GetSpriteIdByName ("wallfrontSTONE");
				TowerFront.SetSprite(spriteNum);
				spriteNum = TowerBack.GetSpriteIdByName ("wallbackSTONE");
				TowerBack.SetSprite(spriteNum);
				
				tk2dSprite StoneSprite = TowerRoof.GetComponent<tk2dSprite>();
				spriteNum = StoneSprite.GetSpriteIdByName("towerSTONE2");
				StoneSprite.SetSprite(spriteNum);

				for (int x = 0; x < TowerPrefabs.Length; x++)
				{
					StoneSprite = TowerPrefabs[x].GetComponent<tk2dSprite>();
					spriteNum = StoneSprite.GetSpriteIdByName("towerSTONE1");
					StoneSprite.SetSprite(spriteNum);
				}
				
			break;
					
		}
	}
}
