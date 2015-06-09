using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour {

	private Transform targetTransform;
	private Camera playerCamera;
	// Use this for initialization
	void Start () {
		targetTransform = this.transform;
		playerCamera = this.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		playerCamera.transform.LookAt (targetTransform.position);
	}
}
