using UnityEngine;
using System.Collections;

public class CameraLerp : MonoBehaviour {

	public GameObject Player;
	public float Smooth = .03f;

	// Use this for initialization
	void Start () 
	{

	}

	void OnJoinedRoom()
	{
		 Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Player != null)
		{
			float z = this.transform.position.z;
			this.transform.position = Vector3.Lerp(this.transform.position, Player.transform.position, Smooth);
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, z);
		}
		else
		{
			Player = GameObject.FindGameObjectWithTag ("Player");
		}
	}
}
