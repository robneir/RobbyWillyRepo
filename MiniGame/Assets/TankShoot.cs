using UnityEngine;
using System.Collections;

public class TankShoot : MonoBehaviour {

    public Transform shellSpawn;
    public GameObject shellPrefab;
    public float shellSpeed;
    public ParticleSystem shootExplosion;
    public GameObject mainCannon;//need this so you can turn on the mouse follow script

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Fire1"))
        {
            ShootShell();
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
}
