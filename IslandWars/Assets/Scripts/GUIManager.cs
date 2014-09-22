using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
	private static GUIManager instance;
	public static GUIManager getInstance() {
		instance = FindObjectOfType(typeof(GUIManager)) as GUIManager;
		if(instance == null) {
			GameObject obj = new GameObject("GUIManager");
			instance = obj.AddComponent<GUIManager>();
		}
		
		return instance;
	}
	
	public tk2dClippedSprite lifebar;
	public tk2dSprite MusicMark, SFXMark, PeopleName;
	public tk2dSprite GameOver, PauseWindow, StoreWindow, OptionsWindow, Victory, StatsWindow;
	public GameObject InGameWindow, UpgradesWindow, AchievementWindow;
	public tk2dSprite[] CoinsSprite, BPSprite, VictoryBPSprite, WaveSprite, PFBSprite;
	public tk2dSlicedSprite StoreButton;
	public tk2dSlicedSprite[] ItemBtns, AttackSprites;
	public tk2dSpriteCollection Numbers;
	public GameObject StatsButton, StatsUpgradeButton;
	
	private int Coins, BravePoints;
	private bool[] isAvailableItem;
	private bool CanOpenStore = true, isMusicOn = true, isSFXOn = true, canUpgrade = false;
	private Transform selected;
	public bool NoPopUpsDisplayed = false;
		
	#region stats

	public tk2dSprite Icon, UpgradeNum;
	public tk2dTextMesh DMG, RNG, SPD;


	#endregion

	void Start()
	{
		//Coins = LevelManager.getInstance().NumOfCoins;
		//UpdateNumofBP();
	}
	
	void Update()
	{
		if (GameManager.getInstance().gameState == GameManager.Gamestate.MainGame || Application.loadedLevel == 3)
		{
			if (CanOpenStore)
				StoreButton.color = new Color (1,1,1,1);
			else 
			{
				StoreButton.color = new Color (1,1,1,0.25f);
				CheckAvailableItems();
			}

			UpdateNumber("Coins", CoinsSprite);
		}
	}
	
	public void ClickedPlayButton()
	{
		if (NoPopUpsDisplayed)
		{
			GameManager.getInstance().gameState = GameManager.Gamestate.Loading;		
		}
	}

	public void ClickedCreditsButton()
	{
		//if (NoPopUpsDisplayed)
	}
	
	public void ClickedPauseButton()
	{
		if (NoPopUpsDisplayed)
		{
			NoPopUpsDisplayed = false;
			Time.timeScale = 0;
			CanOpenStore = true;
			StoreWindow.gameObject.active = false;
			PauseWindow.gameObject.active = true;
		}
	}
	
	public void ClickedResumeButton()
	{
		NoPopUpsDisplayed = true;
		Time.timeScale = 1;
		PauseWindow.gameObject.active = false;
	}
	
	public void ClickedMainMenuButton()
	{
		Time.timeScale = 1;
		GameManager.getInstance().gameState = GameManager.Gamestate.MainMenu;
	}

	public void ClickedRetryButton()
	{
		Time.timeScale = 1;

		//LevelManager.getInstance ().NumOfCoins = 50;
		//LevelManager.getInstance().ResetLevel();
		//LevelManager.getInstance().WaveNum = 1;	
		//StartCoroutine(GUIManager.getInstance().PrepareForBattle());
		GameManager.getInstance().gameState = GameManager.Gamestate.Loading;
	}

	public void ClickedNextLevel()
	{
		Time.timeScale = 1;
		NoPopUpsDisplayed = true;

		Victory.gameObject.active = false;
		LevelManager.getInstance().ResetLevel();
		
		int currentWave = LevelManager.getInstance().WaveNum;
		LevelManager.getInstance().WaveNum = currentWave + 1;
		UpdateNumber("Wave", WaveSprite);

		StartCoroutine(GUIManager.getInstance().PrepareForBattle());
		
	}

	public void ClickedStatsButton()
	{
		if (NoPopUpsDisplayed)
		{
			NoPopUpsDisplayed = false;
			Time.timeScale = 0;
			StatsWindow.gameObject.active = true;
			HideStoreWindow();
		
			ResetStatsWindow();

			int i = 0;
			selected = InputManager.getInstance().getCurrentlySelected();
					
			switch (selected.GetComponent<PeopleScript>().AttackState)
			{
				case "Nearest": 
					i = AttackSprites[0].GetSpriteIdByName("nearest2");  
					AttackSprites[0].SetSprite(i);
				break;

				case "Farthest": 
					i = AttackSprites[1].GetSpriteIdByName("farthest2");  
					AttackSprites[1].SetSprite(i);
				break;

				case "Weakest": 
					i = AttackSprites[2].GetSpriteIdByName("weakest2");  
					AttackSprites[2].SetSprite(i);
				break;

				case "Strongest": 
					i = AttackSprites[3].GetSpriteIdByName("strongest2");  
					AttackSprites[3].SetSprite(i);
				break;
			}

			PeopleScript P = selected.GetComponent<PeopleScript>();
			int Damage = P.WeaponPrefab.GetComponent<WeaponScript>().Damage + P.UpgradeDamage;
			DMG.text = "DMG - " + Damage.ToString();
			DMG.Commit();
			RNG.text = "RNG - " + ((int)Vector2.Distance(selected.position, P.ListOfTarget.transform.position)).ToString();
			RNG.Commit();
			SPD.text = "SPD - " + selected.GetComponent<tk2dSpriteAnimator>().ClipFps.ToString();
			SPD.Commit();

			int level = 0;
			string IconName = "";

			switch (selected.name)
			{
				case "Archer(Clone)": 
					level = PlayerInfo.getInstance().SaveArcherUpgrade; 
					PeopleName.SetSprite(PeopleName.GetSpriteIdByName("archername"));
					
					switch (P.CurrentLevel)
					{
						case 1: IconName = "arrowbow"; break;
						case 2: IconName = "arrowup1"; break;
						case 3: IconName = "arrowup2"; break;
						case 4: IconName = "arrowrate"; break;
					}

				break;

				case "BlowGun(Clone)": 
					level = PlayerInfo.getInstance().SaveBlowGunUpgrade; 
					PeopleName.SetSprite(PeopleName.GetSpriteIdByName("blowgunname"));
					
				switch (P.CurrentLevel)
					{
					case 1: IconName = "blowpipeup"; break;
					case 2: IconName = "blowammoup"; break;
					case 3: IconName = "blowpoison1"; break;
					case 4: IconName = "blowpoison2"; break;
					}

				break;

				case "Slingshot(Clone)": 
					level = PlayerInfo.getInstance().SaveSlingshotUpgrade;
					PeopleName.SetSprite(PeopleName.GetSpriteIdByName("slingername"));

					switch (P.CurrentLevel)
					{
					case 1: IconName = "slingband"; break;
					case 2: IconName = "slingbrace"; break;
					case 3: IconName = "slingammoup"; break;
					case 4: IconName = "slingrate"; break;
					}

				break;

				case "Cannon(Clone)": 
					level = PlayerInfo.getInstance().SaveCannonUpgrade;
					PeopleName.SetSprite(PeopleName.GetSpriteIdByName("cannonname"));

					switch (P.CurrentLevel)
					{
					case 1: IconName = "cannongunpowder"; break;
					case 2: IconName = "cannonball1"; break;
					case 3: IconName = "cannonball2"; break;
					case 4: IconName = "cannonblastradius"; break;
					}

				break;
			}

			int iconNum = Icon.GetSpriteIdByName(IconName);
			Icon.SetSprite(iconNum);
			int UpNum = UpgradeNum.GetSpriteIdByName(P.CurrentLevel.ToString());
			UpgradeNum.SetSprite(UpNum);

			if (P.CurrentLevel < level) DisplayStatsUpgradeButton();
			else HideStatsUpgradeButton();
			
		}

	}

	public void ClickedAttackButton(tk2dUIItem Button)
	{
		ResetStatsWindow ();

		int i = 0;
		//selected = InputManager.getInstance().getCurrentlySelected();
		
		switch (Button.name)
		{
		case "NearestButton": 
			i = AttackSprites[0].GetSpriteIdByName("nearest2");  
			AttackSprites[0].SetSprite(i);
			selected.GetComponent<PeopleScript>().AttackState = "Nearest";
			
			break;
			
		case "FarthestButton": 
			i = AttackSprites[1].GetSpriteIdByName("farthest2");  
			AttackSprites[1].SetSprite(i); 
			selected.GetComponent<PeopleScript>().AttackState = "Farthest";
			
			break;
			
		case "WeakestButton": 
			i = AttackSprites[2].GetSpriteIdByName("weakest2");  
			AttackSprites[2].SetSprite(i);
			selected.GetComponent<PeopleScript>().AttackState = "Weakest";
			
			break;
			
		case "StrongestButton": 
			i = AttackSprites[3].GetSpriteIdByName("strongest2"); 
			AttackSprites[3].SetSprite(i);
			selected.GetComponent<PeopleScript>().AttackState = "Strongest";
			
			break;
		}			
		
	}

	public void ClickedStatsUpgradeButton()
	{
		if (canUpgrade)
		{
			PeopleScript P = selected.GetComponent<PeopleScript>();

			StoreManager.getInstance().UpgradingPerson(P.Cost);

			int currentLvl = P.CurrentLevel;

			if (currentLvl < 4) P.CurrentLevel += 1;

			switch (P.CurrentLevel)
			{
				case 1: P.UpgradeDamage += 1; break;
				case 2: P.UpgradeDamage += 1; break;
				//case 3: Debug.Log ("uhoh"); break;
				//case 4:  selected.GetComponent<tk2dSpriteAnimator>().ClipFps += 2; break;
			}

			NoPopUpsDisplayed = true;
			ClickedStatsButton();
		}

	}

	public void ClickedStatsCancelButton()
	{
		NoPopUpsDisplayed = true;
		Time.timeScale = 1;
		StatsWindow.gameObject.active = false;

	}

	public void ClickedUpgradesButton()
	{
		if (UpgradesWindow != null)
		{	
			UpgradesWindow.active = true;
		}
		else GameManager.getInstance().LoadUpgradesWindow();

		if (Application.loadedLevelName == "prototype")
		{
			Victory.gameObject.active = false;
			InGameWindow.active = false;
		}
	}

	public void ClickedAchievementButton()
	{
		if (AchievementWindow != null)
		{	
			AchievementWindow.active = true;
		}
		else GameManager.getInstance().LoadAchievementWindow();

	}

	public void ClickedOptionsButton()
	{
		NoPopUpsDisplayed = false;
	
		if (PauseWindow != null)
		{
			PauseWindow.gameObject.active = false;
			Victory.gameObject.active = false;
			GameOver.gameObject.active = false;
		}
	
		OptionsWindow.gameObject.active = true;
	}

	public void ClickedBackButton()
	{
		if (OptionsWindow != null)
			OptionsWindow.gameObject.active = false;

		if (PauseWindow == null)
			NoPopUpsDisplayed = true;
		else
			PauseWindow.gameObject.active = true;

	}

	public void ClickedMusicCheckBox()
	{
		if (!isMusicOn)
		{
			isMusicOn = true;
			MusicMark.gameObject.active = true;
		}
		else 
		{
			isMusicOn = false;
			MusicMark.gameObject.active = false;
		}
	}

	public void ClickedSFXCheckBox()
	{
		if (!isSFXOn)
		{
			isSFXOn = true;
			SFXMark.gameObject.active = true;
		}
		else 
		{
			isSFXOn = false;
			SFXMark.gameObject.active = false;
		}
	}

	public void ClickedStoreButton()
	{	
		if (NoPopUpsDisplayed)
		{	
			if (CanOpenStore)
			{	
				CheckAvailableItems();
				StoreWindow.gameObject.active = true;
				HideStatsBUtton();
				CanOpenStore = false;
			}
			/*else 
			{
				StoreWindow.gameObject.active = false;
				CanOpenStore = true;
			}*/
		}
	}
	
	public void ClickedBlowGunButton()
	{
		if (isAvailableItem[0])
		{
			StoreManager.getInstance().Buying(0);
			StoreWindow.gameObject.active = false;
			CanOpenStore = true;
		}
	}
	
	public void ClickedSlingshotButton()
	{
		if (isAvailableItem[1])
		{
			StoreManager.getInstance().Buying(1);
			StoreWindow.gameObject.active = false;
			CanOpenStore = true;
		}
	}
	
	public void ClickedArcherButton()
	{
		if (isAvailableItem[2])
		{
			StoreManager.getInstance().Buying(2);
			StoreWindow.gameObject.active = false;
			CanOpenStore = true;
		}
	}
	
	public void ClickedCannonButton()
	{
		if (isAvailableItem[3])
		{
			StoreManager.getInstance().Buying(3);
			StoreWindow.gameObject.active = false;
			CanOpenStore = true;
		}
	}
	
	public void setLifeBar(int Damage)
	{	
		float CalculatedDamage = Damage * (float)(1.0 / Tower.getInstance ().MaxLife);
		lifebar.clipTopRight = new Vector2 (lifebar.clipTopRight.x - CalculatedDamage, lifebar.clipTopRight.y);
	}

	public void DisplayStatsButton()
	{
		StatsButton.gameObject.active = true;
	}

	public void DisplayStatsUpgradeButton()
	{
		//Transform selected = InputManager.getInstance().getCurrentlySelected();

		StatsUpgradeButton.gameObject.active = true;
		tk2dSlicedSprite Button = StatsUpgradeButton.transform.FindChild("Upgrade").GetComponent<tk2dSlicedSprite>();

		if (StoreManager.getInstance().CanUpgrade(selected.GetComponent<PeopleScript>().Cost))
		{
			canUpgrade = true;
			Button.color = new Color(1,1,1,1);
		}
		else
		{
			canUpgrade = false;
			Button.color = new Color(1,1,1,0.5f);
		}
	}

	public void HideStatsUpgradeButton()
	{
		StatsUpgradeButton.gameObject.active = false;
	}

	public void HideStatsBUtton()
	{
		StatsButton.gameObject.active = false;
	}

	public void HideStoreWindow()
	{
		CanOpenStore = true; 
		StoreWindow.gameObject.active = false;
	}

	public void displayVictory()
	{	
		//if (NoPopUpsDisplayed)
		//{
			NoPopUpsDisplayed = false;
			
			Time.timeScale = 0;
			Victory.gameObject.active = true;
			//StoreWindow.gameObject.active = false;
			HideStoreWindow();
			HideStatsBUtton();

			BravePoints += LevelManager.getInstance().WaveNum;
			PlayerInfo.getInstance().SaveBravePoint = BravePoints;
			UpdateNumber("BravePoint", BPSprite);			
			UpdateNumber("VictoryBP", VictoryBPSprite);
		//}
	}

	public void displayGameOver()
	{
		NoPopUpsDisplayed = false;
		//GameOver.renderer.enabled = true;
		Time.timeScale = 0;
		GameOver.gameObject.active = true;
		//StoreWindow.gameObject.active = false;
		HideStoreWindow();
		HideStatsBUtton();
	}
	
	public void CheckAvailableItems()
	{
		isAvailableItem = StoreManager.getInstance().AvailableItems();
		
		for (int x = 0; x < isAvailableItem.Length; x++)
		{
			if (isAvailableItem[x])
				ItemBtns[x].color = new Color(1,1,1,1);
			else ItemBtns[x].color = new Color(1,1,1,0.5f);
		}
	}
	
	public void UpdateNumofCoins()
	{
		Coins = LevelManager.getInstance().NumOfCoins;

		string CoinsString = Coins.ToString();

		char[] C = CoinsString.ToCharArray();

		for (int x = 0; x < CoinsSprite.Length; x++)
		{
			CoinsSprite[x].renderer.enabled = false;
		}

		for (int x = 0; x < C.Length; x++)
		{	
			CoinsSprite[x].spriteId = CoinsSprite[x].GetSpriteIdByName(C[x].ToString());
			CoinsSprite[x].renderer.enabled = true;
		}
	}

	public void UpdateNumber(string instance, tk2dSprite[] SpriteArray)
	{				
		int Number = 0;

		switch(instance)
		{
			case "Coins":
				Coins = LevelManager.getInstance().NumOfCoins;
				Number = Coins;
			break;

			case "BravePoint":
				BravePoints = PlayerInfo.getInstance ().SaveBravePoint;
				Number = BravePoints;
			break;

			case "VictoryBP":
				Number = LevelManager.getInstance().WaveNum;
			break;

			case "Wave":
				Number = LevelManager.getInstance().WaveNum;
			break;
		}

		string String = Number.ToString();
		
		char[] C = String.ToCharArray();
		
		for (int x = 0; x < SpriteArray.Length; x++)
		{
			SpriteArray[x].renderer.enabled = false;
		}
		
		for (int x = 0; x < C.Length; x++)
		{	
			SpriteArray[x].spriteId = SpriteArray[x].GetSpriteIdByName(C[x].ToString());
			SpriteArray[x].renderer.enabled = true;
		}
	}

	public void ResetStatsWindow()
	{
		int i = 0;
		
		for (int x = 0; x < AttackSprites.Length; x++)
		{
			switch (x)
			{
			case 0: i = AttackSprites[0].GetSpriteIdByName("nearest"); break;
			case 1: i = AttackSprites[1].GetSpriteIdByName("farthest"); break;	
			case 2: i = AttackSprites[2].GetSpriteIdByName("weakest"); break;
			case 3: i = AttackSprites[3].GetSpriteIdByName("strongest"); break;			
			}
			
			AttackSprites[x].SetSprite(i);
		}
	}

	public IEnumerator PrepareForBattle()
	{	
		NoPopUpsDisplayed = false;

		if (PFBSprite != null)
		{	
			for (int x = 0; x < PFBSprite.Length; x++)
			{	
				PFBSprite[x].renderer.enabled = true;
				yield return new WaitForSeconds(1f);
				PFBSprite[x].renderer.enabled = false;

				if (x == PFBSprite.Length-1)
				{
					LevelManager.getInstance ().startSailing = true;
					NoPopUpsDisplayed = true;
				}
			}
		}

	}

}