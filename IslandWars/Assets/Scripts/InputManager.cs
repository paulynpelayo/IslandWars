using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private GameObject player;
	private bool clicked = false;

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

		if (Input.GetMouseButton(0))
		{	
			player = GetClickedGameObject();
			if (player != null && player.tag == "Player")
				clicked = true;
		}

		if (clicked)
			OnMouseDrag();

		if (Input.GetButtonUp("Fire1"))
			clicked = false;
	}

	void OnMouseDrag()
	{
		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);		
		
		point.z = 0;

		player.transform.position = point;
	}

	GameObject GetClickedGameObject()
	{

		//Ray2D ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//RaycastHit2D hit;
		//int layerMask =0;

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if (hit.collider != null)
		{	
			return hit.transform.gameObject;
		}
		else
			return null;
	}
}
