using UnityEngine;
using System.Collections;

public class TankShoot : MonoBehaviour {

    public Transform shellSpawn;
    public float shellSpeed;
    public GameObject tankShell;
    public GameObject mainCannon;//need this so you can turn on the mouse follow script
    public GameObject smallCannon;//need this so you can turn on the mouse follow script

	public Transform bulletSpawn;
    public float bulletSpeed;
    public int bulletDamage;
	public ParticleSystem muzzleFlash;
	public int turretDamage = 5;
    

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () 
	{
	    if(Input.GetButtonDown("Fire1"))
        {
            this.GetComponent<PhotonView>().RPC("ShootShell", PhotonTargets.All);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            PhotonView pv = this.GetComponent<TankMovement>().currentDriver.GetComponentInParent<PhotonView>();
            if(pv.isMine)
            {
                Vector3 sendV = new Vector3(bulletSpawn.right.x * this.transform.root.localScale.x, bulletSpawn.right.y, bulletSpawn.right.z) * bulletSpeed;
                float lScale = this.transform.root.localScale.x;
                //Shoot using rob's method
                pv.RPC("FireBullet2", PhotonTargets.All, PhotonNetwork.player.ID, bulletSpawn.position, bulletSpawn.rotation, bulletSpawn.right, bulletDamage, bulletSpeed);
            }
        }
    }

    [RPC]
    void ShootShell()
    {
        GameObject shell = (GameObject)Instantiate(tankShell, shellSpawn.position, shellSpawn.rotation);
        shell.GetComponent<Rigidbody2D>().AddForce(shellSpawn.right * shellSpeed);
        ParticleSystem part = (ParticleSystem)Instantiate(muzzleFlash, shellSpawn.position, shellSpawn.rotation);
        part.transform.SetParent(shellSpawn);
        part.Play();
    }
}
