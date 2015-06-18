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

	void OnTriggerStay2D(Collider2D col)
	{
		GameObject go= col.gameObject;
		if (go.tag == "Player") 
		{
			player=go.transform.parent.gameObject;//Get parent because that is when the player movement script is
			if(!player.GetComponent<PlayerMovement>().isOnLadder && Input.GetAxis("Vertical")!=0)
			{
				player.GetComponent<PlayerMovement>().isOnLadder=true;
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			player.GetComponent<PlayerMovement>().isOnLadder=false;
		}
	}
}
