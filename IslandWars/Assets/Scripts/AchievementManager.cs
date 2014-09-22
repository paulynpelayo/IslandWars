using UnityEngine;
using System.Collections;

public class AchievementManager : MonoBehaviour {

	public bool achieve = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		tk2dTextMesh text = GetComponent<tk2dTextMesh> ();
		tk2dSprite sprite = GetComponent<tk2dSprite> ();
		if(!achieve)
		{

			if(text) {
				Color textColor = text.color;
				textColor.a = 0.5f;
				text.color = textColor;
			}

			
			if(sprite) {
				Color spriteColor = sprite.color;
				spriteColor.a = 0.5f;
				sprite.color = spriteColor;
			}

		

		}
		else {
			if(text) {
				Color textColor = text.color;
				textColor.a = 1.0f;
				text.color = textColor;
			}
			
			
			if(sprite) {
				Color spriteColor = sprite.color;
				spriteColor.a = 1.0f;
				sprite.color = spriteColor;
			}
		}

}
}
