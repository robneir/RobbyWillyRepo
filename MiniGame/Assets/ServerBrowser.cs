using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerBrowser : MonoBehaviour {

    public GameObject serverBrowserPanel;
    private RoomInfo[] rooms = new RoomInfo[0];

	// Use this for initialization
	void Start () {
        //Connect when press login button
    }

    public void Connect()
    {
        //This connects to photon server using the settings specified in PhotonServerSettings under resources
        PhotonNetwork.ConnectUsingSettings("FirstVersion");
    }

    void OnJoinedLobby()
    {
        Debug.Log("On joined lobby");
    }

    void OnReceivedRoomListUpdate()
    {
        Debug.Log(PhotonNetwork.GetRoomList().Length);
        rooms = PhotonNetwork.GetRoomList();
    }

    void OnGUI()
    {
        //Print connection state in top left corner
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        if (PhotonNetwork.insideLobby == true)
        {
            //Create entire browser in middle of screen
            GUI.BeginGroup(new Rect(Screen.width/2,Screen.height/2, 500, 300));
            //Give option to create room
            GUILayout.BeginHorizontal();
            string roomName = "room name";
            if (GUILayout.Button("Create room"))
            {
                if(roomName==string.Empty)
                {
                    Debug.Log("Need room name to create room");
                }else
                {
                    PhotonNetwork.CreateRoom(roomName);
                }
            }
            GUILayout.EndHorizontal();

            //Print rooms
            if (rooms.Length > 0)
            {
                //Random join button
                if (GUILayout.Button("Join random room"))
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                //Room list
                GUILayout.BeginScrollView(new Vector2(100, 100), GUIStyle.none);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Join?", GUILayout.Width(150));
                GUILayout.Label("Room Name", GUILayout.Width(150));
                GUILayout.Label("# Players", GUILayout.Width(150));
                GUILayout.EndHorizontal();
                foreach (RoomInfo game in rooms)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Join"))
                    {
                        PhotonNetwork.JoinRoom(game.name);
                    }
                    GUILayout.Label(game.name);
                    GUILayout.Label("" + game.playerCount + "/" + game.maxPlayers);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            GUI.EndGroup();
        }
    }

    // Update is called once per frame
    void Update () {
        //if refresh is pressed then grab the rooms again
	}

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log ("Joined room failed");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 4;
        PhotonNetwork.CreateRoom("Robs_game", roomOptions, null);
        Debug.Log("Created my own room");
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log ("Joined random room failed");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 4;
        PhotonNetwork.CreateRoom("Robs_game", roomOptions, null);
        PhotonNetwork.LoadLevel("Game_Scene_UI_Test");
        Debug.Log("Created my own room");
    }

    void OnCreatedRoom()
    {
        Debug.Log("Created room");
        PhotonNetwork.LoadLevel("Game_Scene_UI_Test");
    }

    void OnJoinedRoom()
    {
        Debug.Log ("Joined my own room");
        PhotonNetwork.LoadLevel("Game_Scene_UI_Test");
    }
}
