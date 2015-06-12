using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.L)) {
			this.GetComponent<Rigidbody2D>().isKinematic=false;
			this.GetComponent<Rigidbody2D>().fixedAngle=false;
			this.GetComponentInChildren<Animator>().enabled=false;
		}
	}
}
