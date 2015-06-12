using UnityEngine;
using System.Collections;

public class SpeedPowerUp : MonoBehaviour {

	public float speedMultiplier=2;
	public float duration=6;
	private GameObject player;
	private bool isInitiated=false;
	private float startTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isInitiated) 
		{
			if(Time.time-startTime>duration)
			{
				RemovePowerUp();
			}
		}
	}

	public void UsePowerUp ()
	{
		if (player != null) {
			isInitiated=true;
			player.GetComponent<PlayerMovement> ().speed *= speedMultiplier;
			player.GetComponentInChildren<Animator>().speed*=speedMultiplier;
			startTime = Time.time;
		}
	}

	public void RemovePowerUp ()
	{
		if (player != null)
		{
			player.GetComponent<PlayerMovement> ().speed /= speedMultiplier;
			player.GetComponentInChildren<Animator>().speed/=speedMultiplier;
		}
		GameObject.Destroy (this);
	}

	public void OnCollisionEnter2D (Collision2D c)
	{
		if (c.gameObject.tag != "Player")
			return;
		this.GetComponent<SpriteRenderer> ().enabled = false;
		this.GetComponent<BoxCollider2D> ().enabled = false;
		player = c.gameObject;
		UsePowerUp ();
	}
}
