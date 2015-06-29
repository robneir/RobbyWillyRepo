using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour {

	// Use this for initialization
	void Awake() 
	{
		// see if we've got game music still playing
		GameObject gameMusic = GameObject.Find("GameMusic");
		if (gameMusic != null) 
		{
			// kill game music
			GameObject.Destroy(gameMusic);
		}
		// make sure we survive going to different scenes
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
