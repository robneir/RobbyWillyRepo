using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour { //THIS IS monobehavior with extra photon stuff

	Vector3 realPosition;
	Quaternion realRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			//do nothing
		} else {
			//lerping position and rotation 
			transform.position = Vector3.Lerp (transform.position, realPosition, .1f); /*go 10% closer each time. Could also 
			predict where the player will be by adding velocity of player*/
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, .1f);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			//this is our player. we need to send our actual position to the network
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(transform.localScale);
		} else {
			//this is someone elses player. We need to recieve their position and update our version of that player
			realPosition=(Vector3)stream.ReceiveNext();
			realRotation= (Quaternion)stream.ReceiveNext();
		}
	}
}
