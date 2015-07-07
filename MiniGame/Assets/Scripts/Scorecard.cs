using UnityEngine;
using System.Collections;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;

public class Scorecard : Photon.MonoBehaviour 
{
	public bool FuckThis;

	[HideInInspector]
	public string playerName;

	[HideInInspector]
	public int Kills;
	[HideInInspector]
	public int Deaths;
	[HideInInspector]
	public int Assists;

	HashTable scores;

	//[RPC]
	public void PlayerKilled(int killerID, int deathID)
	{
			if (scores == null)
				CreateScoreHash ();

			//check to see if we were the killer
			if(killerID == PhotonNetwork.player.ID)
			{
				//good job
				Kills++;
				SetScores();
			}
			//if not, did we die?
			else if(deathID == PhotonNetwork.player.ID)
			{
				//we died
				Deaths++;
				SetScores();
			}
	}

	public string ToStringScore()
	{
		return null;//userName + "(ID: " + ID.ToString() + "): K: " + Kills.ToString() + " D: " + Deaths.ToString() + " A: " + Assists.ToString();
	}

	void SetScores()
	{
		scores ["Kills"] = Kills;
		scores ["Deaths"] = Deaths;
		scores ["Assists"] = Assists;
		PhotonNetwork.player.SetCustomProperties (scores);
	}

	// Use this for initialization
	void CreateScoreHash () 
	{
		scores = new HashTable ();

		scores.Add ("Kills", Kills);
		scores.Add ("Deaths", Deaths);
		scores.Add ("Assists", Assists);

		PhotonNetwork.player.SetCustomProperties (scores);
	}
}
