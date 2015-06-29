using UnityEngine;
using System.Collections;

public class GameMusic : MonoBehaviour 
{	
	void Awake() 
	{
		// see if we've got menu music still playing
		GameObject menuMusic = GameObject.Find("MenuMusic");
		if (menuMusic != null) 
		{
			// kill menu music
			GameObject.Destroy(menuMusic);
		}
		// make sure we survive going to different scenes
		DontDestroyOnLoad(gameObject);
	}
}
