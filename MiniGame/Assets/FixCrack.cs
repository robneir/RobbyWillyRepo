using UnityEngine;
using System.Collections;

public class FixCrack : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		this.transform.position = new Vector3 ((int)this.transform.position.x, (int)this.transform.position.y, (int)this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
