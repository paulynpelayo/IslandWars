using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


public class ScrollingScript : MonoBehaviour
{
	public Vector2 speed = new Vector2(10, 10);
	public Vector2 direction = new Vector2(-1, 0);
	public bool isLooping = false;

	private List<Transform> backgroundPart;

	IEnumerator ChangeScene()
	{
		yield return new WaitForSeconds(5f);
		AsyncOperation Async  = Application.LoadLevelAsync("prototype");
		while(!Async.isDone)
		{
			//transform.position = new Vector3(transform.position.x, 14f - (28f * Async.progress),0);
			yield return null;
			Debug.Log("Enter Async");
		}


	}

	void Start()
	{	
		if(Application.loadedLevelName == "LoadingScene")
		{
			StartCoroutine(ChangeScene());
			Debug.Log ("Loading");
		}


		if (isLooping)
		{
			backgroundPart = new List<Transform>();
			
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);

				if (child.renderer != null)
				{
					backgroundPart.Add(child);
				}
			}			

			backgroundPart = backgroundPart.OrderBy(
				t => t.position.x
				).ToList();
		}
	}
	
	void Update()
	{
		Vector3 movement = new Vector3(speed.x * direction.x, speed.y * direction.y, 0);
		
		movement *= Time.deltaTime;
		transform.Translate(movement);
		
		if (isLooping)
		{
			Transform firstChild = backgroundPart.FirstOrDefault();
			
			if (firstChild != null)
			{
				if (firstChild.position.x < Camera.main.transform.position.x)
				{	
					if (firstChild.renderer.IsVisibleFrom(Camera.main) == false)
					{
						Transform lastChild = backgroundPart.LastOrDefault();
						Vector3 lastPosition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.renderer.bounds.max - lastChild.renderer.bounds.min);

						firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);
						
						backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);
					}
				}
			}
		}
	}
}