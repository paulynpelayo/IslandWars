using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private GameObject player, grid;
	private bool clicked = false;
	private Vector2 Scale;

	private Transform CurrentlySelected;

	#region singleton
	
	private static InputManager instance;
	public static InputManager getInstance() {
		instance = FindObjectOfType(typeof(InputManager)) as InputManager;
		if(instance == null) {
			GameObject obj = new GameObject("InputManager");
			instance = obj.AddComponent<InputManager>();
		}

		return instance;
	}

	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0))
		{	
			player = RaycastedObject();
			Debug.Log(player);
			if (player != null && player.tag == "Player")
			{
				clicked = true;
				CurrentlySelected = player.transform;
				GUIManager.getInstance().HideStoreWindow();
				GUIManager.getInstance().DisplayStatsButton();
			}
			else if (player == null)
			{
				CurrentlySelected = null;
				GUIManager.getInstance().HideStatsBUtton();
				GUIManager.getInstance().HideStoreWindow();
			}
		}

		if (clicked)
		{	
			grid = RaycastedObject();

			if (grid != null && grid.tag == "Grid")
			{
				Vector3 point = grid.transform.position;
				point.z = 0;
				
				player.transform.position = point;

				player.GetComponent<PeopleScript>().playerstate = PeopleScript.Playerstate.Moving;
			}
		}

		if (Input.GetButtonUp("Fire1") && clicked)
		{
			clicked = false;
			player.GetComponent<PeopleScript>().playerstate = PeopleScript.Playerstate.Idle;
		}

	}

	GameObject RaycastedObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Physics.Raycast(ray,out hit))
			return hit.transform.gameObject;			
		else return null;

	}

	public Transform getCurrentlySelected()
	{
		return CurrentlySelected;
	}
}
