using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public int Damage = 0;
    public ParticleSystem hitExplosion;

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
        ParticleSystem hitExplo = (ParticleSystem)Instantiate(hitExplosion, this.transform.position, this.transform.rotation);
        hitExplo.Play();
        PhotonNetwork.Destroy(this.gameObject);
    }
}
