using UnityEngine;
using System.Collections;

public class TankShoot : MonoBehaviour {

    public Transform shellspawn;
    public GameObject shellPrefab;

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
       GameObject shell=  (GameObject) Instantiate(shellPrefab, shellspawn.position, shellspawn.rotation);
       shell.GetComponent<Rigidbody2D>().AddForce(shellspawn.forward * 1000);
    }
}
