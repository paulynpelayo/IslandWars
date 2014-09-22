using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {



	public AudioClip playsound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter() {


			audio.PlayOneShot(playsound, 1.0F);


	}
}
