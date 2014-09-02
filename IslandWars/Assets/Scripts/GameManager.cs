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
		PauseMenu
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


	// Use this for initialization
	void Start () {
		if (Application.loadedLevel == 0)
			gameState = Gamestate.TeamLogo;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator WaitToChangeScene()
	{
		yield return new WaitForSeconds(2f);
		gameState = Gamestate.MainMenu;
	}

	IEnumerator ChangeScene()
	{
		AsyncOperation Async  = Application.LoadLevelAsync(1);
		while(!Async.isDone)
		{
			transform.position = new Vector3(transform.position.x, 14f - (28f * Async.progress),0);
			yield return null;
		}
		//yield return new WaitForSeconds();
		
	}

	void ChangeState()
	{
		switch (_gamestate)
		{	
			case Gamestate.TeamLogo:

			StartCoroutine(WaitToChangeScene());

			break;

			case Gamestate.MainMenu:

			//Application.LoadLevelAsync("MainMenu");
			Application.LoadLevel(1);

			break;

			case Gamestate.Loading:

			//Application.LoadLevelAsync("LoadingScene");
			Application.LoadLevel(2);
			gameState = Gamestate.MainGame;

			break;

			case Gamestate.MainGame:

			//Application.LoadLevelAsync("prototype");
			Application.LoadLevel(3);


			break;
		}
	}
}
