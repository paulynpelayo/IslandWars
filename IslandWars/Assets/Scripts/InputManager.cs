using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public WeaponScript weapon;
	private GameObject player;
	private bool clicked = false;
	private Vector2 Scale;


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
			player = GetClickedGameObject();
			if (player != null && player.tag == "Player")
				clicked = true;
		}

		if (clicked)
		{
			OnMouseDrag();
			player.GetComponent<PeopleScript>().playerstate = PeopleScript.Playerstate.Moving;
		}

		if (Input.GetButtonUp("Fire1"))
		{
			clicked = false;
			player.GetComponent<PeopleScript>().playerstate = PeopleScript.Playerstate.Idle;
		}

	}

	void OnMouseDrag()
	{
		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);		
		
		point.z = 0;

		player.transform.position = point;

		//to be change by Bon! :D
		if (player.transform.position.x > 0)
		{
			Scale = new Vector2 (1, 1);
			player.transform.localScale = Scale;
		}
		else if (player.transform.position.x < 0)
		{
			Scale = new Vector2 (-1, 1);
			player.transform.localScale = Scale;
		}
	}

	GameObject GetClickedGameObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Physics.Raycast(ray,out hit))
			return hit.transform.gameObject;			
		else return null;

	}
}
