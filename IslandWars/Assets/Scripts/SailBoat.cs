using UnityEngine;
using System.Collections;

public class SailBoat : MonoBehaviour {

	public Transform TargetPoint;
	public float TimeToSail = 3f;
	public bool isSailing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (LevelManager.getInstance().startSailing)
		{
			ReadyToSail();
			isSailing = true;
		}

		if (isSailing)
		{
			if (transform.position == TargetPoint.position)
			{
				gameObject.GetComponent<tk2dSpriteAnimator>().Stop();
				LevelManager.getInstance().readytoSpawn = true;
				isSailing = false;
			}
		}

	}

	void ReadyToSail()
	{		
		transform.position = Vector3.MoveTowards(transform.position, TargetPoint.position, TimeToSail * Time.deltaTime * 0.1f);
	}
	
}
