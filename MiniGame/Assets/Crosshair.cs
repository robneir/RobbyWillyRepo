using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y,0);
	}
}
