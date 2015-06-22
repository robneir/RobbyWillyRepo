using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public int Damage = 0;
    public GameObject hitExplosion;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionEnter2D(Collision2D  c)
	{
		if(c.gameObject.tag.Equals("Player"))
		{
			PlayerHealth sb = c.gameObject.GetComponent<PlayerHealth>();
			sb.TakeDamage(Damage);
        }
		if(!c.gameObject.tag.Equals("Bullet"))
		{
			if(hitExplosion != null)
			{
	        	GameObject.Destroy((GameObject)Instantiate(hitExplosion, this.transform.position, this.transform.rotation), 5f);
			}
			else Debug.Log("Var HitExplosion is null.");
	        PhotonNetwork.Destroy(this.gameObject);
		}
    }

	void OnTriggerExit2D(Collider2D c)
	{
		if(c.gameObject.tag == "LEVEL_BOUNDS")
		{
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
}
