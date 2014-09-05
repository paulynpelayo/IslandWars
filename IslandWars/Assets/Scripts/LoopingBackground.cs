using UnityEngine;
using System.Collections;

public class LoopingBackground : MonoBehaviour {
	
	public float scrollSpeed;


	void Update()
	{	
	
		Debug.Log (renderer.materials [0].mainTextureOffset);
	renderer.materials[0].mainTextureOffset -= new Vector2 (0,scrollSpeed * Time.deltaTime);
		/*
		if(renderer.material.mainTextureOffset.y + offset2 >= 1)
		{
			//renderer.material.SetTextureOffset("_MainTex",Vector2.zero);
			renderer.material.mainTextureOffset = Vector2.zero;
		}*/
	}
	
}