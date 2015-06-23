using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoToMain : MonoBehaviour {

	public Text input;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		if (GUI.Button (new Rect(Screen.width / 2 - 80, Screen.height / 2 - 15, 160,30), "Test Alpha"))
		{
			PhotonNetwork.playerName = input.text;

			if(input.text == "")
				PhotonNetwork.playerName = GenerateRandomUsername();

			Application.LoadLevel("Game_Scene");
		}
	}

	string GenerateRandomUsername()
	{
		string x = "User";

		for(int i = 0; i < 6; ++i)
		{
			x += Random.Range (0, 9).ToString();
		}

		return x;
	}
}
