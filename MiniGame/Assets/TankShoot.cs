using UnityEngine;
using System.Collections;

public class TankShoot : MonoBehaviour {

    public Transform shellSpawn;
    public float shellSpeed;
    public ParticleSystem shootExplosion;
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
            ShootShell();
        }
		else if(Input.GetButtonDown("Fire2"))
		{
			ShootTurret();
		}
	}

    void ShootShell()
    {
       GameObject shell=  (GameObject) PhotonNetwork.Instantiate("Tank_Shell", shellSpawn.position, shellSpawn.rotation,0);
       shell.GetComponent<Rigidbody2D>().AddForce(shellSpawn.right * shellSpeed);
       ParticleSystem part= (ParticleSystem) Instantiate(shootExplosion, shellSpawn.position, shellSpawn.rotation);
       part.transform.SetParent(shellSpawn);
       part.Play();
    }

	void ShootTurret()
	{
		GameObject.Destroy((GameObject)GameObject.Instantiate(muzzleFlash, this.turretSpawn.position, Quaternion.identity), 1f);	
		
		GameObject bull = (GameObject)PhotonNetwork.Instantiate("Bullet", this.turretSpawn.position, turretSpawn.rotation, 0);	
		bull.GetComponent<Bullet> ().Damage = this.turretDamage;
		bull.GetComponent<Rigidbody2D>().velocity = new Vector3(turretSpawn.right.x, turretSpawn.right.y, turretSpawn.right.z);
		bull.GetComponent<Rigidbody2D>().velocity *= 60;
	}
}
