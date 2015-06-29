using UnityEngine;
using System.Collections;

public class ItemSpawnSpot : MonoBehaviour {

	public int teamId=0;
	public bool readyToSpawn = true;
	public string PrefabName = "Katana";
	public int respawnTimerInSeconds = 60;
	public bool RandomSpawn = false;

	private int respawnTimer = 0;
	private int respawnTimerReset = 0;

	// Use this for initialization
	void Start () 
	{
		respawnTimer = respawnTimerInSeconds * 60;
		respawnTimerReset = respawnTimer;
	}

	public void OnSpawn()
	{
		readyToSpawn = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(!readyToSpawn)
		{
			respawnTimer--;
			
			if(respawnTimer <= 0)
			{
				respawnTimer = respawnTimerReset;
				readyToSpawn = true;
			}
		}
	}
}

