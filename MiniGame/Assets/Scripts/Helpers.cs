using UnityEngine;
using System.Collections;

public static class Helpers
{
	public static GameObject GetMyLocalPlayer()
	{
		GameObject[] pList = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log(pList.Length);
		if(pList.Length > 0)
		{
			foreach(GameObject p in pList)
			{
				//if id of player is same as me
				if(p.GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
				{
					//this is the correct local player.
					return p;
					break;
				}
			}
		}

		return null;
	}
}
