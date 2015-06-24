using UnityEngine;
using System.Collections;

public class TankShoot : Photon.MonoBehaviour {

    public Transform shellSpawn;
    public float shellSpeed;
    public Transform bulletSpawn;
    public float bulletSpeed;
    public ParticleSystem shootExplosion;
    public GameObject tankShellPrefab;
    public GameObject tankBulletPrefab;
    public GameObject mainCannon;//need this so you can turn on the mouse follow script
    public GameObject smallCannon;

    public Transform smallTurretSpawn;
	public GameObject muzzleFlash;
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
            this.GetComponent<PhotonView>().RPC("ShootBullets", PhotonTargets.All);
        }
    }

    [RPC]
    void ShootShell()
    {
        GameObject shell = (GameObject)Instantiate(tankShellPrefab, shellSpawn.position, shellSpawn.rotation);
        shell.GetComponent<Rigidbody2D>().AddForce(shellSpawn.right * shellSpeed);
        ParticleSystem part = (ParticleSystem)Instantiate(shootExplosion, shellSpawn.position, shellSpawn.rotation);
        part.transform.SetParent(shellSpawn);
        part.Play();
    }
    
    void ShootBullets()
    {
        //photonView.RPC("FireBullet", PhotonTargets.All, PhotonNetwork.player.ID, bulletSpawn.position, bulletSpawn.rotation, sendV, this.Damage, lScale);
    }
}
