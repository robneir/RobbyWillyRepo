using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public bool xAxisEnabled=false;
	public bool yAxisEnabled=false;
	public bool zAxisEnabled=false;

	public bool gravityEnabled;
	public float gravityStrength;

	public float speed;
	public float friction;
	public float jumpPower;

	private Rigidbody rigidBody;
	private bool OnGround=true;

	// Use this for initialization
	void Start () {
		rigidBody = this.GetComponent<Rigidbody> ();
	}

	void OnGUI()
	{

	}
	
	// Update is called once per frame
	void Update () {
		float deltaX=0;
		float deltaZ=0;
		if(xAxisEnabled)
			deltaX=Input.GetAxis ("Horizontal") * speed;
		if(zAxisEnabled)
			deltaZ=Input.GetAxis ("Vertical") * speed;
		if (yAxisEnabled) {
			if(Input.GetButtonDown("Jump") && OnGround)
			{
				rigidBody.AddForce(0,this.jumpPower,0);
				OnGround=false;
			}
		}
		transform.position = new Vector3 (deltaX+transform.position.x,
		                                  transform.position.y,
		                                  deltaZ+transform.position.z); 
		
		/*
		float xForce=0;
		float zForce=0;
		float yForce=0;
		if(xAxisEnabled)
			xForce=Input.GetAxis ("Horizontal") * speed;
		if(zAxisEnabled)
			zForce=Input.GetAxis ("Vertical") * speed;
		Vector3 newForceVect = new Vector3 (xForce,yForce,zForce); 
		rigidBody.AddForce(newForceVect);*/
	}

	
	void OnCollisionEnter(Collision col)
	{
		if(col.collider.tag=="Ground" && !OnGround)
		{
			OnGround=true;
		}
	}
}
