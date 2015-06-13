using UnityEngine;
using System.Collections;

public class LadderZone : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		GameObject go= col.gameObject;
		if (go.tag == "Player") 
		{
			player=go.transform.parent.gameObject;//Get parent because that is when the player movement script is
			if(!player.GetComponent<PlayerMovement>().IsOnLadder)
			{
				player.GetComponent<PlayerMovement>().IsOnLadder=true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == player.tag) {
			player.GetComponent<PlayerMovement>().IsOnLadder=false;
		}
	}
}
