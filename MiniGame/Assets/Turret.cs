using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    public GameObject gun;
    public GameObject bulletPrefab;
    public Transform fireTip;
    public float bulletSpeed;
	public int bulletDamage = 25;
    public float timeBetweenShots;
    public AudioClip fireSound;
    public ParticleSystem fireEffect;

    private float lastShotTime;

	private LookAtObject l;
    private AudioSource audioSource;
    private Recoil recoil;

	// Use this for initialization
	void Start () 
	{
        lastShotTime = -timeBetweenShots;
		l = gun.GetComponent<LookAtObject> ();
        audioSource = this.GetComponent<AudioSource>();
        recoil = l.GetComponent<Recoil>();
    }
	
	// Update is called once per frame
	void Update () 
	{
	    if(l.target != null)
        {
			if(PhotonNetwork.isMasterClient)
			{
				if (Time.time - lastShotTime >= timeBetweenShots)
            	{
                    lastShotTime = Time.time;

                    /*FIX THIS SO THAT TURRETFIRES IN OPPOSITE DIRECTION AS WELL*/
                    if(transform.localScale.x>0)
					    this.GetComponent<PhotonView>().RPC("FireTurretBullet", PhotonTargets.All, fireTip.position, fireTip.rotation, fireTip.right, bulletDamage, bulletSpeed);
                    else
                        this.GetComponent<PhotonView>().RPC("FireTurretBullet", PhotonTargets.All, fireTip.position, fireTip.rotation, fireTip.right*-1, bulletDamage, bulletSpeed);
                }

				//check if your target is dead, if he is, make him null
				if(l.target.GetComponent<PlayerStatus>().dead)
				{
					LoseTarget();
				}
            }
        } 
	}

    void OnTriggerStay2D(Collider2D c)
	{
        //If target is a player then make it the new target
        if(l.target == null && c.gameObject.tag == "Player")
        {
            ChangeTarget(c.gameObject);
        }
    }

	void OnTriggerExit2D(Collider2D c)
	{
		//if you have a target and the target is the same person who is leaving your radius
		if(l.target != null && l.target == c.gameObject)
		{
			LoseTarget();
		}
	}

	void LoseTarget()
	{
		//dont have a target anymore
		l.target = null;
		//reset time to shoot
		lastShotTime = Time.time;
	}
	
    void ChangeTarget(GameObject newTarget)
    {
        l.target = newTarget;
    }

    [RPC]
    void FireTurretBullet(Vector3 position, Quaternion rotation, Vector3 direction, int damage, float speed)
    {
        recoil.DoRecoil();
        audioSource.clip = fireSound;
        audioSource.Play();
        ParticleSystem fireEff = (ParticleSystem)Instantiate(fireEffect, fireTip.position, fireTip.rotation);
        fireEffect.Play();
        GameObject bull = (GameObject)Instantiate(bulletPrefab, position, rotation);
        bull.GetComponent<Bullet>().Damage = damage;
		bull.GetComponent<Bullet> ().ID = -1;//turret should have an ID that doesnt match any players ever.
        bull.GetComponent<Rigidbody2D>().AddForce(speed * direction);
    }
}
