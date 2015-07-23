using UnityEngine;
using System.Collections;
using System;

public static class Helpers
{
	public static GameObject GetMyLocalPlayer()
	{
		GameObject[] pList = GameObject.FindGameObjectsWithTag("Player");

		if(pList.Length > 0)
		{
			foreach(GameObject p in pList)
			{
				//if player movement is enabled
				try
				{
					if(p.GetComponent<PlayerMovement>().enabled)
					{
						//this is the correct local player.
						return p;
					}
				}

				catch(NullReferenceException e)
				{
					Debug.Log(e.Message);
					return p;
				}
			}
		}

		return null;
	}

	public static string GetPlayerName(int viewID)
	{
		if(viewID == (int)AI_Constants.ID.Turret)
		{
			return "Turret";
		}
		else if(viewID == (int)AI_Constants.ID.Drone)
		{
			return "Drone";
		}

		for(int i =0; i < PhotonNetwork.playerList.Length; ++i)
		{
			if (PhotonNetwork.player.ID == viewID)
			{
				return "You";
			}
			else if(PhotonNetwork.playerList[i].ID == viewID)
			{
				if (PhotonNetwork.playerList[i].name != "")
				{
					return PhotonNetwork.playerList[i].name;
				}
				else return "Player " + PhotonNetwork.playerList[i].ID.ToString();
			}
		}
		
		return "???";
	}
}
