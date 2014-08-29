using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	private static Tower instance;
	public static Tower getInstance() {
		instance = FindObjectOfType(typeof(Tower)) as Tower;
		if(instance == null) {
			GameObject obj = new GameObject("Tower");
			instance = obj.AddComponent<Tower>();
		}
		return instance;
	}

	private int Life = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (Life);
	}
}
