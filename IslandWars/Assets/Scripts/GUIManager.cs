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
	public tk2dSprite MusicMark, SFXMark;
	public tk2dSprite GameOver, PauseWindow, StoreWindow, OptionsWindow, Victory;
	public GameObject InGameWindow, UpgradesWindow, AchievementWindow;
	public tk2dSprite[] CoinsSprite, PFBSprite;
	public tk2dSlicedSprite StoreButton;
	public tk2dSlicedSprite[] ItemBtns;
	public tk2dSpriteCollection Numbers;
	
	private int Coins;
	private bool[] isAvailableItem;
	private bool CanOpenStore = true, isMusicOn = true, isSFXOn = true;
	public bool NoPopUpsDisplayed = false;
		
	void Start()
	{
		//Coins = LevelManager.getInstance().NumOfCoins;
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

			UpdateNumofCoins();
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
		StartCoroutine(GUIManager.getInstance().PrepareForBattle());
		
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

		if (AchievementWindow == null)
		{
			AchievementWindow = GameObject.Find("AchievementWindow");
			AchievementWindow.gameObject.active = false;
		}
		else AchievementWindow.gameObject.active = false;

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
				CanOpenStore = false;
			}
			else 
			{
				StoreWindow.gameObject.active = false;
				CanOpenStore = true;
			}
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
		//lifebar.clipTopRight = new Vector2 (lifebar.clipTopRight.x - (Damage * 0.01f), lifebar.clipTopRight.y);
		lifebar.clipTopRight = new Vector2 ((lifebar.clipTopRight.x) - (Damage *  (Tower.getInstance().MaxLife * 0.0001f)), lifebar.clipTopRight.y);
	}

	public void displayVictory()
	{	
		//if (NoPopUpsDisplayed)
		//{
			NoPopUpsDisplayed = false;

			Time.timeScale = 0;
			Victory.gameObject.active = true;
			StoreWindow.gameObject.active = false;
		//}
	}

	public void displayGameOver()
	{
		NoPopUpsDisplayed = false;
		//GameOver.renderer.enabled = true;
		Time.timeScale = 0;
		GameOver.gameObject.active = true;
		StoreWindow.gameObject.active = false;
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