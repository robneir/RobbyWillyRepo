using UnityEngine;
using System.Collections;

public class NetworkStatusBar : Photon.MonoBehaviour {

	Vector3 realPosition;
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			//do nothing
		} 
		else 
		{
			//lerping position and rotation 
			transform.position = Vector3.Lerp (transform.position, realPosition, .1f);
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) 
		{
			//this is our player. we need to send our actual position to the network
			stream.SendNext(transform.position);
		} 
		else 
		{
			//this is someone elses player. We need to recieve their position and update our version of that player
			realPosition=(Vector3)stream.ReceiveNext();
		}
	}
}
