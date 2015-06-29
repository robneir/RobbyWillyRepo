using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	//Privates
	private SpawnSpot[] spawnSpots;
    //Player
    private GameObject myPlayerGO;

	// Use this for initialization
	void Start () {
		//Find spawn spot for player
		spawnSpots= GameObject.FindObjectsOfType <SpawnSpot>();
		//turn off physics for collisions between player and item
		//Physics2D.IgnoreLayerCollision (8, 9, true);
		Connect ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void Connect()
	{
		//This connects to photon server using the settings specified in PhotonServerSettings under resources
		PhotonNetwork.ConnectUsingSettings ("FirstVersion");  
	}

	void OnGUI()
	{
		//Print connection state in top left corner
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}

	void OnJoinedLobby()
	{
		Debug.Log ("Joined lobby");
		//Create a room 
		PhotonNetwork.JoinRandomRoom ();
	}

	//If failed to join random room this is executed
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log ("Joined lobby failed");
		PhotonNetwork.CreateRoom("(Room name)");
	}

	void OnJoinedRoom()
	{
		Debug.Log ("Joined room");
		SpawnMyPlayer ();
	}

    void OnPhotonPlayerDisconnected()
    {
        Debug.Log("Left room");
        PhotonView pv = myPlayerGO.GetComponent<PhotonView>();
        if(pv.isMine)
        {
            pv.RPC("DestroyStatusBar", PhotonTargets.AllBuffered);
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
		myPlayerGO= (GameObject) PhotonNetwork.Instantiate ("Bandit",grabbedSpawnSpot.transform.position, 
		                           grabbedSpawnSpot.transform.rotation, 
		                           grabbedSpawnSpot.teamId);
		//Enable and disable player components depending on if they should be seen locally or over network
		//Disabled means seen over network and enabled means needed to be locally seen
		myPlayerGO.GetComponent<PlayerMovement> ().enabled = true;
		myPlayerGO.GetComponent<Rigidbody2D> ().gravityScale = 4;

		PhotonView pv = myPlayerGO.GetComponent<PhotonView> ();

		if (pv.isMine) 
		{
			pv.RPC ("InstantiateHealthBar", PhotonTargets.AllBuffered, 100f);
		}
		//If camera is attached to palyer then: myPlayerGo.transform.FindChild("string of camera").gameobject.setactive(true);
	}
}
