using UnityEngine;
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
	public tk2dTextMesh InfoText, SubInfo, AmountText;

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
		BravePoints = PlayerInfo.getInstance().SaveBravePoint;
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
		/*int buttonNum;
		
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

		/*if(curArcherUpgrades < 4)
		{
			curArcherUpgrades += 1;
		}
		
		ArrArcherUpgrades[curArcherUpgrades].color = new Color(1,1,1,1);*/
	}

	public void ClickedBlowgunUpgrades()
	{
		if (NoPopUpsDisplayed)
		{
			if(curBlowgunUpgrades < 4)
			{
				curBlowgunUpgrades += 1;
			}
			ArrBlowgunUpgrades[curBlowgunUpgrades].color = new Color(1,1,1,1);
		}
	}

	public void ClickedSlingshotUpgrades()
	{
		if (NoPopUpsDisplayed)
		{
			if(curSlingshotUpgrades < 4)
			{
				curSlingshotUpgrades += 1;
			}
			ArrSlingshotUpgrades[curSlingshotUpgrades].color = new Color(1,1,1,1);
		}
	}

	public void ClickedCannonUpgrades()
	{
		if (NoPopUpsDisplayed)
		{
			if(curCannonUpgrades < 4)
			{
				curCannonUpgrades += 1;
			}
			ArrCannonUpgrades[curCannonUpgrades].color = new Color(1,1,1,1);
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
		InfoText.Commit();
		SubInfo.text = Subinfo;
		SubInfo.Commit();
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

	}

	public void UpgradeBlowgunUpgrades()
	{
		
	}

	public void UpgradeSlingshotUpgrades()
	{
		
	}

	public void UpgradeCannonUpgrades()
	{
		
	}
}
