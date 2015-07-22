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
}
