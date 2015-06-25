using UnityEngine;
using System.Collections;

public class TankShoot : MonoBehaviour {

    public Transform shellSpawn;
    public float shellSpeed;
    public ParticleSystem shootExplosion;
    public GameObject tankShell;
    public GameObject mainCannon;//need this so you can turn on the mouse follow script
    public GameObject smallCannon;//need this so you can turn on the mouse follow script

	public Transform turretSpawn;
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
	}

    [RPC]
    void ShootShell()
    {
        GameObject shell = (GameObject)Instantiate(tankShell, shellSpawn.position, shellSpawn.rotation);
        shell.GetComponent<Rigidbody2D>().AddForce(shellSpawn.right * shellSpeed);
        ParticleSystem part = (ParticleSystem)Instantiate(shootExplosion, shellSpawn.position, shellSpawn.rotation);
        part.transform.SetParent(shellSpawn);
        part.Play();
    }
}
