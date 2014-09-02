using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public string sceneToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
