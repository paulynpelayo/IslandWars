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
		Loading
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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator WaitToChangeScene()
	{
		yield return new WaitForSeconds(5f);
	}

	void ChangeState()
	{
		switch (_gamestate)
		{
			case Gamestate.MainGame:

			StartCoroutine(WaitToChangeScene());
			Application.LoadLevelAsync("LoadingScene");

			break;

		}
	}
}
