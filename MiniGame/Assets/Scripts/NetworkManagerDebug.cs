using UnityEngine;
using System.Collections;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;
using System.IO;
using System.Linq;

public class NetworkManagerDebug : MonoBehaviour {
	//Privates
	private SpawnSpot[] spawnSpots;
	private ItemSpawnSpot[] itemSpawnSpots;
	//Player
	private GameObject myPlayerGO;
	private string[] itemNames;
	private int spawnTimer = 5 * 60;//5 sec
	private int spawnTimerReset;
	bool initialSpawn = false;

	// Use this for initialization
	void Start () 
	{
		//Find spawn spot for player
		spawnSpots= GameObject.FindObjectsOfType <SpawnSpot>();
		itemSpawnSpots= GameObject.FindObjectsOfType <ItemSpawnSpot>();
		//turn off physics for collisions between player and item
		//Physics2D.IgnoreLayerCollision (8, 9, true);
		GetPrefabList ();
		spawnTimerReset = spawnTimer;
		//turn off physics for collisions between player and item
		//Physics2D.IgnoreLayerCollision (8, 9, true);
		Connect ();
	}

	void GetPrefabList()
	{    
		DirectoryInfo dir = new DirectoryInfo("Assets/Prefabs/Items/Resources");
		FileInfo[] info = dir.GetFiles("*.prefab");
		itemNames = info.Select(f => f.Name.Replace(".prefab", "")).ToArray();
	}
	
	string GetRandomItemName()
	{
		int r = Random.Range (0, itemNames.Length);
		string toSpawn = itemNames [r];
		
		return toSpawn;
	}

	// Update is called once per frame
	void Update () 
	{
		if(PhotonNetwork.isMasterClient)
		{
			foreach(var i in itemSpawnSpots)
			{
				if(i.readyToSpawn)
				{
					if(i.RandomSpawn)
					{
						PhotonNetwork.Instantiate (GetRandomItemName(), i.transform.position, i.transform.rotation, 0);
						i.OnSpawn();
					}
					else
					{
						PhotonNetwork.Instantiate (i.PrefabName, i.transform.position, i.transform.rotation, 0);
						i.OnSpawn();
					}
				}
			}
		}
		
		if(spawnTimer <= 0)
		{
			spawnTimer = 0;
			
			if(Input.GetKeyDown(KeyCode.E))
			{
				//spawn
				RespawnMyPlayer();
				spawnTimer = spawnTimerReset;
			}
		}
	}

	void Connect()
	{
		//This connects to photon server using the settings specified in PhotonServerSettings under resources
		PhotonNetwork.ConnectUsingSettings ("FirstVersion");  
	}

	void OnJoinedLobby()
	{
		Debug.Log ("Joined lobby");
		//Create a room 
		PhotonNetwork.CreateRoom ("Rob");
	}

	//If failed to join random room this is executed
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log ("Joined lobby failed");
		PhotonNetwork.CreateRoom("(Room name)");
	}

	void SetupHash()
	{
		PhotonHashTable pht = new PhotonHashTable();
		pht["Kills"] = 0;
		pht["Deaths"] = 0;
		pht["Assists"] = 0;
		PhotonNetwork.player.SetCustomProperties(pht);
	}

	void OnJoinedRoom()
	{
		//setup kda
		SetupHash ();
		Debug.Log ("Joined room");
		SpawnMyPlayer ();
	}

	void FixedUpdate()
	{
		ControlSpawnPlayers ();
	}
	
	void ControlSpawnPlayers()
	{
		if(myPlayerGO != null && myPlayerGO.GetComponent<PlayerStatus>().dead)
		{
			spawnTimer--;
		}
	}
	
	void OnGUI()
	{
		//Print connection state in top left corner
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		
		if(myPlayerGO != null && myPlayerGO.GetComponent<PlayerStatus>().dead && initialSpawn)
		{
			if(spawnTimer > 0)
			{
				GUI.Label(new Rect(Screen.width / 2, 150, 300,100), "Can spawn in..." + ((int)(spawnTimer / 60f)).ToString());
			}
			else
			{
				GUI.Label(new Rect(Screen.width / 2, 150, 300,100), "Press E to spawn." + ((int)(spawnTimer / 60f)).ToString());
			}
		}
	}
	
	void OnPhotonPlayerDisconnected()
	{
		Debug.Log("Left room");
		
		if(myPlayerGO != null)
		{
			PhotonView pv = myPlayerGO.GetComponent<PhotonView>();
			//Destory health bar 
			myPlayerGO.GetComponent<PlayerStatus>().Die();
		}
	}
	
	void RespawnMyPlayer()
	{	
		PhotonView pv = myPlayerGO.GetComponent<PhotonView> ();
		
		SpawnSpot grabbedSpawnSpot = spawnSpots [Random.Range (0,spawnSpots.Length)];
		pv.RPC ("RespawnMe", PhotonTargets.All, grabbedSpawnSpot.transform.position, grabbedSpawnSpot.transform.rotation);
		
		//if (pv.isMine) 
		//{
		pv.RPC ("InstantiateHealthBar", PhotonTargets.AllBuffered, 100f);
		//}
	}

	void SpawnMyPlayer()
	{
		if (spawnSpots == null) {
			Debug.LogError ("WTF, There are no spawn spots");
			return;
		}
		//Get random spawnspot
		SpawnSpot grabbedSpawnSpot = spawnSpots [Random.Range (0,spawnSpots.Length)];
		/*This instantiates a player on the network so that everyone has the instantiation
		 * but the prefab must be located in the resource folder
		*/
	 	myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("Bandit",grabbedSpawnSpot.transform.position, 
		                           grabbedSpawnSpot.transform.rotation, 
		                           grabbedSpawnSpot.teamId);
        //Enable and disable player components depending on if they should be seen locally or over network
        //Disabled means seen over network and enabled means needed to be locally seen
        Camera.main.GetComponent<AudioListener>().enabled = false;
		myPlayerGO.GetComponent<PlayerMovement> ().enabled = true;
		myPlayerGO.GetComponent<AudioListener> ().enabled = true;
		myPlayerGO.GetComponent<GravityLiftEffector> ().enabled = true;
		myPlayerGO.GetComponent<Rigidbody2D> ().gravityScale = 4;

		PhotonView pv = myPlayerGO.GetComponent<PhotonView> ();

		if (pv.isMine) 
		{
			pv.RPC ("InstantiateHealthBar", PhotonTargets.AllBuffered, 100f);
		}
		initialSpawn = true;
		//If camera is attached to palyer then: myPlayerGo.transform.FindChild("string of camera").gameobject.setactive(true);
	}
}
