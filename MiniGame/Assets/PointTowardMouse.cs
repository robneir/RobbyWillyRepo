using UnityEngine;
using System.Collections;

public class PointTowardMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mouseDiff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        float rotation = Mathf.Atan2(mouseDiff.y, mouseDiff.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,rotation);
	}
}
