using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class NetworkManager : MonoBehaviour {
	//Privates
	private SpawnSpot[] spawnSpots;
	private ItemSpawnSpot[] itemSpawnSpots;
    //Player
    private GameObject myPlayerGO;
	private string[] itemNames;

    void Awake()
    {
        //Find spawn spot for player
        spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
    }
    
	// Use this for initialization
	void Start () {
		itemSpawnSpots= GameObject.FindObjectsOfType <ItemSpawnSpot>();
		//turn off physics for collisions between player and item
		//Physics2D.IgnoreLayerCollision (8, 9, true);
		GetPrefabList ();
        //Connect ();
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
	}

	void OnGUI()
	{
		//Print connection state in top left corner
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}
    
    void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        //PhotonNetwork.isMessageQueueRunning = true;
        SpawnMyPlayer();
    }

    
    void OnJoinedRoom()
	{
		Debug.Log ("Joined room");
        SpawnMyPlayer();
    }

    void OnPhotonPlayerDisconnected()
    {
        Debug.Log("Left room");

		if(myPlayerGO != null)
		{
        	PhotonView pv = myPlayerGO.GetComponent<PhotonView>();
	        if(pv.isMine)
	        {
	            pv.RPC("DestroyStatusBar", PhotonTargets.AllBuffered,100f);
	        }
		}
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
        Camera.main.GetComponent<AudioListener>().enabled = false;
		myPlayerGO= (GameObject) PhotonNetwork.Instantiate ("Bandit",grabbedSpawnSpot.transform.position, 
		                           grabbedSpawnSpot.transform.rotation, 
		                           grabbedSpawnSpot.teamId);
		//Enable and disable player components depending on if they should be seen locally or over network
		//Disabled means seen over network and enabled means needed to be locally seen
		myPlayerGO.GetComponent<PlayerMovement> ().enabled = true;
		myPlayerGO.GetComponent<AudioListener> ().enabled = true;
		myPlayerGO.GetComponent<Rigidbody2D> ().gravityScale = 4;

		PhotonView pv = myPlayerGO.GetComponent<PhotonView> ();

		if (pv.isMine) 
		{
			pv.RPC ("InstantiateHealthBar", PhotonTargets.AllBuffered, 100f);
		}
		//If camera is attached to palyer then: myPlayerGo.transform.FindChild("string of camera").gameobject.setactive(true);
	}
}
