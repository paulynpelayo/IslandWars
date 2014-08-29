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

	void Awake()
	{

	}
	
	void Start()
	{

	}
	
	public void ClickedPlayButton()
	{
		Debug.Log("Clicked");
		GameManager.getInstance ().gameState = GameManager.Gamestate.MainGame;
	}

	public void ClickedOptionsButton()
	{
		
	}

	public void ClickedCreditsButton()
	{
		
	}
	

}