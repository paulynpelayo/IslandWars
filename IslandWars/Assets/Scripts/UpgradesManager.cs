using UnityEngine;
using System.Collections;

public class UpgradesManager : MonoBehaviour {

	public tk2dSlicedSprite[] ArrTowerHeight;
	public tk2dSlicedSprite[] ArrTowerDefense;
	public tk2dSlicedSprite[] ArrArcherUpgrades;
	public tk2dSlicedSprite[] ArrBlowgunUpgrades;
	public tk2dSlicedSprite[] ArrSlingshotUpgrades;
	public tk2dSlicedSprite[] ArrCannonUpgrades;

	private int curTowerHeight;
	private int curTowerDefense;
	private int curArcherUpgrades;
	private int curBlowgunUpgrades;
	private int curSlingshotUpgrades;
	private int curCannonUpgrades;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Add Functions on Upgrades
	public void ClickedTowerHeight()
	{
		Tower.getInstance().increaseLevel();

		if(curTowerHeight < 4)
		{
			curTowerHeight += 1;
			//Debug.Log(curTowerHeight);
		}

		ArrTowerHeight[curTowerHeight].color = new Color(1,1,1,1);
	}

	public void ClickedTowerDefense()
	{
		Tower.getInstance().increaseLevel();

		if(curTowerDefense < 4)
		{
			curTowerDefense += 1;
		}
		
		ArrTowerDefense[curTowerDefense].color = new Color(1,1,1,1);
	}

	public void ClickedArcherUpgrades()
	{
		if(curArcherUpgrades < 4)
		{
			curArcherUpgrades += 1;
		}
		
		ArrArcherUpgrades[curArcherUpgrades].color = new Color(1,1,1,1);
	}

	public void ClickedBlowgunUpgrades()
	{
		if(curBlowgunUpgrades < 4)
		{
			curBlowgunUpgrades += 1;
		}
		ArrBlowgunUpgrades[curBlowgunUpgrades].color = new Color(1,1,1,1);
	}

	public void ClickedSlingshotUpgrades()
	{
		if(curSlingshotUpgrades < 4)
		{
			curSlingshotUpgrades += 1;
		}
		ArrSlingshotUpgrades[curSlingshotUpgrades].color = new Color(1,1,1,1);
	}

	public void ClickedCannonUpgrades()
	{
		if(curCannonUpgrades < 4)
		{
			curCannonUpgrades += 1;
		}
		ArrCannonUpgrades[curCannonUpgrades].color = new Color(1,1,1,1);
	}

}
