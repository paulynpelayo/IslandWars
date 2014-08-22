using UnityEngine;
using System.Collections;

public class parallax : MonoBehaviour {

	private float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<MeshRenderer> ().material.mainTextureOffset = new Vector2 (speed, 0);
		speed += 0.00025f;
	}
}
