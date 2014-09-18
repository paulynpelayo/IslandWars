using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	#region singleton
	

	public enum Gamestate
	{
		TeamLogo,
		MainMenu, 
		OptionsMenu,
		Credits,
		MainGame,
		Loading,
		PauseMenu,
		Upgrades
	}
	private Gamestate _gamestate;	
	public Gamestate gameState
	{
		get { return _gamestate; }
		set
		{
			_gamestate = value;
			ChangeState();
		}
	}

	#endregion

	private static GameManager instance;
	public static GameManager getInstance() {
		instance = FindObjectOfType(typeof(GameManager)) as GameManager;
		if(instance == null) {
			GameObject obj = new GameObject("GameManager");
			instance = obj.AddComponent<GameManager>();
		}
		
		return instance;
	}

	public Transform BG;

	// Use this for initialization
	void Start () {
		if (Application.loadedLevel == 0)
			gameState = Gamestate.TeamLogo;

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (_gamestate);
	}

	IEnumerator WaitToChangeScene()
	{
		yield return new WaitForSeconds(2f);
		gameState = Gamestate.MainMenu;
	}

	IEnumerator ChangeScene()
	{	
		AsyncOperation Async  = Application.LoadLevelAsync("prototype");
		while(!Async.isDone)
		{
			BG.position = new Vector3(BG.position.x, 14f - (28f * Async.progress),0);
			//yield return null;
		}
		yield return new WaitForSeconds(2f);
	
	}

	void ChangeState()
	{
		switch (_gamestate)
		{	
			case Gamestate.TeamLogo:

				StartCoroutine(WaitToChangeScene());

			break;

			case Gamestate.MainMenu:

				Application.LoadLevelAsync("MainMenu");
				//Application.LoadLevel(1);

			break;

			case Gamestate.Loading:

				//Application.LoadLevelAsync("LoadingScene");
				Application.LoadLevel(2);

			//gameState = Gamestate.MainGame;
			

			break;

			case Gamestate.MainGame:

				Application.LoadLevel(3);

				//StartCoroutine(ChangeScene());

			break;
		}
	}

	public void LoadUpgradesWindow()
	{
		Application.LoadLevelAdditive(4);
	}

	public void LoadAchievementWindow()
	{
		Application.LoadLevelAdditive(5);
	}
}
