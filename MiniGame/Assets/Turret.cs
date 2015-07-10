using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform fireTip;
    public float bulletSpeed;

    public float timeBetweenShots;

    private float lastShotTime;

	// Use this for initialization
	void Start () {
        lastShotTime = -timeBetweenShots;
    }
	
	// Update is called once per frame
	void Update () {
	    if(this.GetComponent<LookAtObject>().target!=null)
        {
            if (Time.time - lastShotTime >= timeBetweenShots)
            {
                try
                {
                    if(PhotonNetwork.inRoom)
                    {
                        lastShotTime = Time.time;
                        this.GetComponent<PhotonView>().RPC("FireBullet2", PhotonTargets.AllBuffered, fireTip.position, fireTip.rotation, fireTip.right, bulletPrefab.GetComponent<Bullet>().Damage, bulletSpeed);
                    }
                }
                catch(MissingReferenceException ex)
                {
                    Debug.Log("Missing a target, Turret has no target to shoot at! :"+ex.ToString());
                }
            }
        }
        
	}

    void OnTriggerStay2D(Collider2D c)
    {
        //If player is already targeted then dont target another player until 
        if (this.GetComponent<LookAtObject>().target.tag=="Player")
        {
            return;
        }

        //If target is a player then make it the new target
        if(c.tag=="Player")
        {
            ChangeTarget(c.gameObject);
        }
    }

    void ChangeTarget(GameObject newTarget)
    {
        this.GetComponent<LookAtObject>().target = newTarget;
    }

    [RPC]
    void FireBullet2(Vector3 position, Quaternion rotation, Vector3 direction, int damage, float speed)
    {
        GameObject bull = (GameObject)Instantiate(bulletPrefab, position, rotation);
        bull.GetComponent<Bullet>().Damage = damage;
        bull.GetComponent<Rigidbody2D>().AddForce(speed * direction);
    }
}
