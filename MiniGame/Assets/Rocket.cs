using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

	public int ID = -1;
	public int MaxDamage = 120;
	public int MinRadius = 10;
	public ParticleSystem hitExplosion;
	
	private Rigidbody2D rigidBody2D;
	
	public bool rotateEffectOn;
	public float rotateSpeed;

	void Start () 
	{
		rigidBody2D=this.GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		if (rotateEffectOn)
		{
			transform.Rotate(0, 0, rotateSpeed);
		}
	}

	void Collisions(Collision2D c)
	{
		//if you are the shooter
		if(ID == PhotonNetwork.player.ID)
		{
			//handle the damaging across network
			GameObject[] pList = GameObject.FindGameObjectsWithTag("Player");

			foreach(var p in pList)
			{
				p.gameObject.GetComponent<PhotonView>().RPC("RocketDamage", PhotonTargets.AllBuffered,
				                                            MaxDamage, MinRadius, ID, p.GetComponent<PhotonView>().ownerId, this.transform.position);
			}
		}

		Destroy(this.gameObject);
		//PhotonNetwork.Destroy(this.gameObject);


		if(hitExplosion != null)
		{
			Instantiate(hitExplosion, new Vector3(transform.position.x,transform.position.y,transform.position.z), this.transform.rotation);
		}
		else Debug.Log("Var HitExplosion is null.");
	}
	
	void OnCollisionEnter2D(Collision2D  c)
	{
		Collisions (c);
	}

	void OnCollisionStay2D(Collision2D  c)
	{
		Collisions (c);
	}
	
	void OnTriggerExit2D(Collider2D c)
	{
		if(c.gameObject.tag == "LEVEL_BOUNDS")
		{
			Destroy(this.gameObject);
		}
	}
}
