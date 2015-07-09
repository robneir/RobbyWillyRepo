using UnityEngine;
using System.Collections;
using System;

public class NetworkCharacter : Photon.MonoBehaviour { //THIS IS monobehavior with extra photon stuff

	Vector3 realPosition;
	Quaternion realRotation;
    Vector3 localScale;
	private Animator animator;

	//all for the arms..some other script is fucking with this. just let it be like this for now
	public Transform armNear;
	public Transform armFar;
	Quaternion anRot;
	Quaternion afRot;
	private Quaternion storedAN = Quaternion.identity;
	private Quaternion storedAF = Quaternion.identity;

	PlayerMovement thisMovement;

	// Use this for initialization
	void Start () 
	{
		animator = transform.GetChild(0).GetComponent<Animator> ();
		//armNear = transform.GetChild (0).transform.GetChild (1).transform.GetChild (2).gameObject;
		//armFar = transform.GetChild (0).transform.GetChild (1).transform.GetChild (0).gameObject;

		thisMovement = GetComponent<PlayerMovement> ();
	}

	void Update () 
	{
		if (photonView.isMine) 
		{
			//do nothing
		} 
		else 
		{
			//lerping position and rotation 
			transform.position = Vector3.Lerp (transform.position, realPosition, .1f); /*go 10% closer each time. Could also 
			predict where the player will be by adding velocity of player*/
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, .1f);
			transform.localScale = localScale;
			storedAN = Quaternion.Lerp(storedAN, anRot, .15f);
			storedAF = Quaternion.Lerp(storedAF, afRot, .15f);
		}
	}
	
	// LateUpdate is called once per frame
	void LateUpdate () 
	{
		if (!photonView.isMine) 
		{
			armNear.rotation = storedAN; //Quaternion.Lerp(armNear.rotation, anRot, .1f);
			armFar.rotation = storedAF;
		}
	}

	void OnFootSend(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(transform.position);
		stream.SendNext(transform.rotation);
		stream.SendNext(transform.localScale);
		try
		{
			stream.SendNext(animator.GetFloat("Speed"));
		}
		catch
		{
			stream.SendNext(0);
			Debug.Log("Could not send running speed data.");
		}
		try
		{
			stream.SendNext(animator.GetBool("Jump"));
		}
		catch
		{
			stream.SendNext(false);
			Debug.Log("Could not send jump data.");
		}
		try
		{
			stream.SendNext(animator.speed);
		}
		catch
		{
			stream.SendNext(1);
			Debug.Log("Could not send animator speed data.");
		}
		stream.SendNext (armNear.rotation);
		stream.SendNext (armFar.rotation);
	}

	void OnFootReceive(PhotonStream stream, PhotonMessageInfo info)
	{
		//this is someone elses player. We need to recieve their position and update our version of that player
		realPosition=(Vector3)stream.ReceiveNext();
		realRotation= (Quaternion)stream.ReceiveNext();
		localScale = (Vector3)stream.ReceiveNext();
		animator.SetFloat("Speed", (float)stream.ReceiveNext());
		animator.SetBool("Jump", (bool)stream.ReceiveNext());
		animator.speed = (float)stream.ReceiveNext();
		anRot = (Quaternion)stream.ReceiveNext ();
		afRot = (Quaternion)stream.ReceiveNext ();
	}

	void InTankSend(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(transform.position);
	}
	
	void InTankReceive(PhotonStream stream, PhotonMessageInfo info)
	{
		//this is someone elses player. We need to recieve their position and update our version of that player
		realPosition = (Vector3)stream.ReceiveNext ();
	}
		
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) 
		{
			OnFootSend(stream, info);
        } 
        else 
        {
			OnFootReceive(stream, info);
		}
	}
}
