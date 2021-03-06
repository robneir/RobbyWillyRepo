﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public int ID = -1;
	public int Damage = 0;
    public ParticleSystem hitExplosion;

    private Rigidbody2D rigidBody2D;
    
    public bool rotateEffectOn;
    public float rotateSpeed;

    // Use this for initialization
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
		if(c.gameObject.tag.Equals("Player"))
		{
			//only send the takeDamage to eveeryone on one client
			if(c.gameObject.GetComponent<PhotonView>().ownerId == PhotonNetwork.player.ID)
			{
				PlayerStatus sb = c.gameObject.GetComponent<PlayerStatus>();
				c.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, Damage, ID, c.gameObject.GetComponent<PhotonView>().ownerId);
				Destroy(this.gameObject);
			}
		}
		else if(ID != (int)AI_Constants.ID.Drone && c.gameObject.tag.Equals("Drone"))
		{
			//if you are the shooter
			if(ID == PhotonNetwork.player.ID)
			{
				c.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, Damage, ID);
				Destroy(this.gameObject);
			}
		}
		if(c.gameObject.tag != "Bullet")
		{
			if(hitExplosion != null)
			{
				Instantiate(hitExplosion, new Vector3(transform.position.x,transform.position.y,transform.position.z), this.transform.rotation);
			}
			else Debug.Log("Var HitExplosion is null.");
			
			Destroy(this.gameObject);
			//PhotonNetwork.Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D  c)
	{
		Collisions (c);
    }

    void OnCollisionStay2D(Collision2D c)
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
