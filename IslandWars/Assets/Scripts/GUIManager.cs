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
	public tk2dSprite GameOver, PauseWindow, StoreWindow;
	public tk2dSprite[] CoinsSprite;
	public tk2dSprite[] PFBSprite;
	public tk2dSlicedSprite StoreButton;
	public tk2dSlicedSprite[] ItemBtns;
	public tk2dSpriteCollection Numbers;
	
	private int Coins;
	private bool[] isAvailableItem;
	private bool CanOpenStore = true;
	
	void Start()
	{
		Coins = LevelManager.getInstance().NumOfCoins;
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
		//Debug.Log("Clicked");
		GameManager.getInstance().gameState = GameManager.Gamestate.Loading;
	}
	
	public void ClickedOptionsButton()
	{
		
	}
	
	public void ClickedCreditsButton()
	{
		
	}
	
	public void ClickedPauseButton()
	{
		Time.timeScale = 0;
		//GameManager.getInstance().Gamestate = GameManager.Gamestate.PauseMenu;
		PauseWindow.gameObject.active = true;
	}
	
	public void ClickedResumeButton()
	{
		Time.timeScale = 1;
		PauseWindow.gameObject.active = false;
	}
	
	public void ClickedMainMenuButton()
	{
		Time.timeScale = 1;
		GameManager.getInstance().gameState = GameManager.Gamestate.MainMenu;
	}
	
	public void ClickedStoreButton()
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
		lifebar.clipTopRight = new Vector2 (lifebar.clipTopRight.x - (Damage * 0.01f), lifebar.clipTopRight.y);
	}
	
	public void displayGameOver()
	{
		//GameOver.renderer.enabled = true;
		GameOver.gameObject.active = true;
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
		if (PFBSprite != null)
		{
			for (int x = 0; x < PFBSprite.Length; x++)
			{
				PFBSprite[x].renderer.enabled = true;
				yield return new WaitForSeconds(1f);
				PFBSprite[x].renderer.enabled = false;

				if (x == PFBSprite.Length-1)
					LevelManager.getInstance ().startWave = true;
			}
		}

	}

}