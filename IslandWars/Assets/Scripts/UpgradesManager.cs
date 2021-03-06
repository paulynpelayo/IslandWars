﻿using UnityEngine;
using System.Collections;

public class UpgradesManager : MonoBehaviour {

	public tk2dSlicedSprite[] ArrTowerHeight;
	public tk2dSlicedSprite[] ArrTowerDefense;
	public tk2dSlicedSprite[] ArrArcherUpgrades;
	public tk2dSlicedSprite[] ArrBlowgunUpgrades;
	public tk2dSlicedSprite[] ArrSlingshotUpgrades;
	public tk2dSlicedSprite[] ArrCannonUpgrades;

	public tk2dSlicedSprite PurchaseButton;
	public tk2dSprite InfoWindow, Icon;
	public tk2dSprite[] BPSprite;
	public tk2dTextMesh InfoText, SubInfo, InfoTextBack, SubInfoBack,AmountText;

	private int curTowerHeight;
	private int curTowerDefense;
	private int curArcherUpgrades;
	private int curBlowgunUpgrades;
	private int curSlingshotUpgrades;
	private int curCannonUpgrades;

	private int BravePoints, Amount = 100000;
	private bool canPurchase = false, NoPopUpsDisplayed = true;
	private string ButtonClicked;

	// Use this for initialization
	void Start () {

		GUIManager.getInstance().UpgradesWindow = this.gameObject;
		//BravePoints = PlayerInfo.getInstance().SaveBravePoint;
		UpdateNumofBP();

		curTowerHeight = PlayerInfo.getInstance().SaveTowerHeight;
		curTowerDefense = PlayerInfo.getInstance().SaveTowerDefense;
		curArcherUpgrades = PlayerInfo.getInstance().SaveArcherUpgrade;
		curBlowgunUpgrades = PlayerInfo.getInstance().SaveBlowGunUpgrade;
		curSlingshotUpgrades = PlayerInfo.getInstance().SaveSlingshotUpgrade;
		curCannonUpgrades = PlayerInfo.getInstance().SaveCannonUpgrade;

		DisplayUpgrades (ArrTowerHeight, curTowerHeight);
		DisplayUpgrades (ArrTowerDefense, curTowerDefense);
		DisplayUpgrades (ArrArcherUpgrades, curArcherUpgrades);
		DisplayUpgrades (ArrBlowgunUpgrades, curBlowgunUpgrades);
		DisplayUpgrades (ArrSlingshotUpgrades, curSlingshotUpgrades);
		DisplayUpgrades (ArrCannonUpgrades, curCannonUpgrades);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UpdateNumofBP()
	{				
		BravePoints = PlayerInfo.getInstance ().SaveBravePoint;
		//Debug.Log (BravePoints);
		string BPString = BravePoints.ToString();
		
		char[] C = BPString.ToCharArray();
		
		for (int x = 0; x < BPSprite.Length; x++)
		{
			BPSprite[x].renderer.enabled = false;
		}
		
		for (int x = 0; x < C.Length; x++)
		{	
			BPSprite[x].spriteId = BPSprite[x].GetSpriteIdByName(C[x].ToString());
			BPSprite[x].renderer.enabled = true;
		}
	}

	public void DisplayUpgrades(tk2dSlicedSprite[] Item, int currentLvl)
	{	
		for (int x = 0; x < currentLvl; x++)
		{
			Item[x].color = new Color(1,1,1,1);
		}
	}
	

	//Add Functions on Upgrades
	public void ClickedTowerHeight(tk2dUIItem Button)
	{
		int buttonNum;
		int.TryParse(Button.gameObject.name, out buttonNum);

		if (NoPopUpsDisplayed)
		{
			int amount = 0;

			switch(buttonNum)
			{
				case 1: amount = 10; break;
				case 2: amount = 50; break;
				case 3: amount = 150; break;
				case 4: amount = 300; break;
			}

			int iconID = ArrTowerHeight[buttonNum - 1].spriteId;
			Icon.SetSprite(iconID);

			//Upgrades Details Window
			string Info = "Level " + buttonNum;
			string Subinfo = "+1 Deck";
			DisplayInfoWindow("TowerHeight", Info, Subinfo, amount);
		}

	}

	public void ClickedTowerDefense(tk2dUIItem Button)
	{
		int buttonNum;

		int.TryParse(Button.gameObject.name, out buttonNum);

		if (NoPopUpsDisplayed)
		{
			int amount = 0;
			string Info = "";
			string Subinfo = "";
			
			switch(buttonNum)
			{
			case 1: 
				amount = 10; 
				Info = "Building Techniques";
				Subinfo = "+10 Life";
				break;
			case 2: 
				amount = 50;
				Info = "Hardwood Panels";
				Subinfo = "+20 Life";
				break;
			case 3: 
				amount = 150;
				Info = "Stone Defense";
				Subinfo = "+50 Life";
				break;
			case 4: 
				amount = 300;
				Info = "Masonry";
				Subinfo = "+75 Life";
				break;
			}

			int iconID = ArrTowerDefense[buttonNum - 1].spriteId;
			Icon.SetSprite(iconID);

			DisplayInfoWindow("TowerDefense", Info, Subinfo, amount);
		}
	}

	public void ClickedArcherUpgrades(tk2dUIItem Button)
	{
		int buttonNum;
		
		int.TryParse(Button.gameObject.name, out buttonNum);
		
		if (NoPopUpsDisplayed)
		{
			int amount = 0;
			string Info = "";
			string Subinfo = "";
			
			switch(buttonNum)
			{
			case 1: 
				amount = 10; 
				Info = "Longbow";
				Subinfo = "+5 Range";
				break;
			case 2: 
				amount = 50;
				Info = "Bodkin Tip";
				Subinfo = "+1 Damage";
				break;
			case 3: 
				amount = 150;
				Info = "Broadhead Tip";
				Subinfo = "+1 Damage";
				break;
			case 4: 
				amount = 300;
				Info = "Archery Practice";
				Subinfo = "+1 Rate of Fire";
				break;
			}
			
			int iconID = ArrArcherUpgrades[buttonNum - 1].spriteId;
			Icon.SetSprite(iconID);
			
			DisplayInfoWindow("ArcherUpgrade", Info, Subinfo, amount);
		}

	}

	public void ClickedBlowgunUpgrades(tk2dUIItem Button)
	{
		int buttonNum;
		
		int.TryParse(Button.gameObject.name, out buttonNum);
		
		if (NoPopUpsDisplayed)
		{
			int amount = 0;
			string Info = "";
			string Subinfo = "";
			
			switch(buttonNum)
			{
			case 1: 
				amount = 10; 
				Info = "Longer Blowpipe";
				Subinfo = "Range";
				break;
			case 2: 
				amount = 50;
				Info = "Special Dart";
				Subinfo = "Damage";
				break;
			case 3: 
				amount = 150;
				Info = "Venum";
				Subinfo = "Damage";
				break;
			case 4: 
				amount = 300;
				Info = "Poison";
				Subinfo = "Damage";
				break;
			}
			
			int iconID = ArrBlowgunUpgrades[buttonNum - 1].spriteId;
			Icon.SetSprite(iconID);
			
			DisplayInfoWindow("BlowgunUpgrade", Info, Subinfo, amount);
		}

	}

	public void ClickedSlingshotUpgrades(tk2dUIItem Button)
	{
		int buttonNum;
		
		int.TryParse(Button.gameObject.name, out buttonNum);
		
		if (NoPopUpsDisplayed)
		{
			int amount = 0;
			string Info = "";
			string Subinfo = "";
			
			switch(buttonNum)
			{
			case 1: 
				amount = 10; 
				Info = "Heavyband";
				Subinfo = "Range";
				break;
			case 2: 
				amount = 50;
				Info = "Armbrace";
				Subinfo = "Damage";
				break;
			case 3: 
				amount = 150;
				Info = "Larger Projectile";
				Subinfo = "Damage";
				break;
			case 4: 
				amount = 300;
				Info = "Sling Pratice";
				Subinfo = "Rate of Fire";
				break;
			}
			
			int iconID = ArrSlingshotUpgrades[buttonNum - 1].spriteId;
			Icon.SetSprite(iconID);
			
			DisplayInfoWindow("SlingshotUpgrade", Info, Subinfo, amount);
		}

	}

	public void ClickedCannonUpgrades(tk2dUIItem Button)
	{
		int buttonNum;
		
		int.TryParse(Button.gameObject.name, out buttonNum);
		
		if (NoPopUpsDisplayed)
		{
			int amount = 0;
			string Info = "";
			string Subinfo = "";
			
			switch(buttonNum)
			{
			case 1: 
				amount = 10; 
				Info = "Gunpowder";
				Subinfo = "Range";
				break;
			case 2: 
				amount = 50;
				Info = "Metal Cannonballs";
				Subinfo = "Damage";
				break;
			case 3: 
				amount = 150;
				Info = "Exploding Shells";
				Subinfo = "Damage";
				break;
			case 4: 
				amount = 300;
				Info = "Blast Radius";
				Subinfo = "Damage";
				break;
			}
			
			int iconID = ArrCannonUpgrades[buttonNum - 1].spriteId;
			Icon.SetSprite(iconID);
			
			DisplayInfoWindow("CannonUpgrade", Info, Subinfo, amount);
		}

	}

	public void ClickedBackButton()
	{
		if (NoPopUpsDisplayed)
		{
			this.gameObject.active = false;

			//if (GameManager.getInstance().gameState == GameManager.Gamestate.MainMenu)
			if (Application.loadedLevelName == "prototype")
			{
				GUIManager.getInstance().InGameWindow.active = true;
				GUIManager.getInstance().displayVictory();
			}
		}
	}

	public void ClickedCancelButton()
	{
		InfoWindow.gameObject.active = false;
		NoPopUpsDisplayed = true;
	}

	public void ClickedPurchaseButton()
	{
		if (canPurchase)
		{
			switch (ButtonClicked)
			{
				case "TowerHeight": UpgradeTowerHeight(); break;
				case "TowerDefense": UpgradeTowerDefense(); break;
				case "ArcherUpgrade": UpgradeArcherUpgrades(); break;
				case "BlowgunUpgrade": UpgradeBlowgunUpgrades(); break;
				case "SlingshotUpgrade": UpgradeSlingshotUpgrades(); break;
				case "CannonUpgrade": UpgradeCannonUpgrades(); break;		 		
			}

			BravePoints -= Amount;
			PlayerInfo.getInstance().SaveBravePoint = BravePoints;
			UpdateNumofBP();
			Amount = 100000;
			ButtonClicked = null;

			InfoWindow.gameObject.active = false;
			NoPopUpsDisplayed = true;
		}
	}
	
	public void DisplayInfoWindow(string button, string Info, string Subinfo, int amount)
	{	
		NoPopUpsDisplayed = false;

		ButtonClicked = button;
		Amount = amount; 

		InfoText.text = Info;
		InfoTextBack.text = Info;
		InfoText.Commit();
		InfoTextBack.Commit();
		SubInfo.text = Subinfo;
		SubInfoBack.text = Subinfo;
		SubInfo.Commit();
		SubInfoBack.Commit();
		AmountText.text = Amount.ToString();
		AmountText.Commit();

		InfoWindow.gameObject.active = true;

		if (Amount <= BravePoints)
		{
			canPurchase = true;
			PurchaseButton.color = new Color(1,1,1,1);
		}
		else 
		{
			canPurchase = false;
			PurchaseButton.color = new Color(1,1,1, 0.5f);
		}

	}

	/// <summary>
	/// Upgrades the height of the tower.
	/// </summary>
	public void UpgradeTowerHeight()
	{
		if(curTowerHeight < 4)
		{			
			ArrTowerHeight[curTowerHeight].color = new Color(1,1,1,1);
			curTowerHeight += 1;
		}
		else curTowerHeight = 4;

		//Save Player Attributes
		PlayerInfo.getInstance().SaveTowerHeight = curTowerHeight;

		if (Application.loadedLevelName == "prototype")
			Tower.getInstance().TowerLevel += 1;
	}


	public void UpgradeTowerDefense()
	{
		if(curTowerDefense < 4)
		{
			ArrTowerDefense[curTowerDefense].color = new Color(1,1,1,1);
			curTowerDefense += 1;
		}
		else curTowerDefense = 4;
		
		//Save Player Attributes
		PlayerInfo.getInstance().SaveTowerDefense = curTowerDefense;
		
		if (Application.loadedLevelName == "prototype")
			Tower.getInstance().TowerDefense += 1;

	}

	public void UpgradeArcherUpgrades()
	{
		if(curArcherUpgrades < 4)
		{
			ArrArcherUpgrades[curArcherUpgrades].color = new Color(1,1,1,1);
			curArcherUpgrades += 1;
		}
		else curArcherUpgrades = 4;

		PlayerInfo.getInstance().SaveArcherUpgrade = curArcherUpgrades;

	}

	public void UpgradeBlowgunUpgrades()
	{
		if(curBlowgunUpgrades < 4)
		{
			ArrBlowgunUpgrades[curBlowgunUpgrades].color = new Color(1,1,1,1);
			curBlowgunUpgrades += 1;
		}
		else curBlowgunUpgrades = 4;

		PlayerInfo.getInstance().SaveBlowGunUpgrade = curBlowgunUpgrades;
			
	}

	public void UpgradeSlingshotUpgrades()
	{
		if(curSlingshotUpgrades < 4)
		{
			ArrSlingshotUpgrades[curSlingshotUpgrades].color = new Color(1,1,1,1);
			curSlingshotUpgrades += 1;
		}
		else curSlingshotUpgrades = 4;

		PlayerInfo.getInstance().SaveSlingshotUpgrade = curSlingshotUpgrades;

	}

	public void UpgradeCannonUpgrades()
	{
		if(curCannonUpgrades < 4)
		{
			ArrCannonUpgrades[curCannonUpgrades].color = new Color(1,1,1,1);
			curCannonUpgrades += 1;
		}
		else curCannonUpgrades = 4;

		PlayerInfo.getInstance().SaveCannonUpgrade = curCannonUpgrades;

	}
}
