using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkScore : Photon.MonoBehaviour {

	//public PhotonPlayer[] PlayerList;
	// Use this for initialization
	void Start () 
	{
		//PlayerList = PhotonNetwork.playerList;
	}

	// Update is called once per frame
	void Update () 
	{
		//if(PlayerList.Length != PhotonNetwork.playerList.Length)
		//{
			//set array
			//PlayerList = PhotonNetwork.playerList;
		//}
	}

	void OnGUI()
	{
		GUI.Label (new Rect (20, Screen.height - (20 * (PhotonNetwork.playerList.Length + 1)), 100, 50), "Current Users: " + PhotonNetwork.playerList.Length.ToString ());

		for(int i = 1; i <= PhotonNetwork.playerList.Length; ++i)
		{
			try
			{
				GUI.Label (new Rect (0, Screen.height - (20 *  i), 100, 50), PhotonNetwork.playerList[i-1].name.ToString() + ": K: " + PhotonNetwork.playerList[i-1].customProperties["Kills"].ToString()
				          + ": D: " + PhotonNetwork.playerList[i-1].customProperties["Deaths"].ToString() + ": ID: " + PhotonNetwork.playerList[i-1].ID.ToString());
				// + ": D: " + PhotonNetwork.playerList[i-1].customProperties["Deaths"].ToString() + ": ID: " + PhotonNetwork.playerList[i-1].ID.ToString());
			}
			catch
			{
				GUI.Label (new Rect (0, Screen.height - (20 *  i), 100, 50), "EMPTY");
			}
		}
	}
}
