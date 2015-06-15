using UnityEngine;
using System.Collections;

public class RPCs : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	[RPC]
	void SyncTrigger(string trigName, GameObject player)
	{
		player.GetComponent<Animator> ().SetTrigger (trigName);
	}
}
