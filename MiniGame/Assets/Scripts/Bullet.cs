using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public int Damage = 0;
    public GameObject hitExplosion;

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

	void OnCollisionEnter2D(Collision2D  c)
	{
		if(c.gameObject.tag.Equals("Player"))
		{
			PlayerHealth sb = c.gameObject.GetComponent<PlayerHealth>();
			c.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, Damage);
        }
		if(c.gameObject.tag!="Bullet")
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

	void OnTriggerExit2D(Collider2D c)
	{
		if(c.gameObject.tag == "LEVEL_BOUNDS")
		{
            Debug.Log("HIT");
			Destroy(this.gameObject);
		}
	}
}
