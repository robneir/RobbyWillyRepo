using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public bool xAxisEnabled=false;
	public bool yAxisEnabled=false;
	public bool zAxisEnabled=false;

	public float speed;
	public float friction;
	public float jumpPower;

	private Rigidbody2D rigidBody2D;
	private bool OnGround=true;

	// Use this for initialization
	void Start () {
		rigidBody2D = this.GetComponent<Rigidbody2D> ();
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
				rigidBody2D.AddForce(new Vector2(0,this.jumpPower));
				OnGround=false;
			}
		}
		transform.position = new Vector3 (deltaX+transform.position.x,
		                                  transform.position.y,
		                                  deltaZ+transform.position.z); 

		//Changing directions
		if ((Input.GetAxisRaw ("Horizontal")>0 && this.transform.localScale.x<0)||
		    ((Input.GetAxisRaw ("Horizontal")<0 && this.transform.localScale.x>0))) 
		{
			this.transform.localScale=new Vector3(this.transform.localScale.x*-1,
			                                      this.transform.localScale.y,
			                                      this.transform.localScale.z);
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.tag=="Ground" && !OnGround)
		{
			OnGround=true;
		}
	}
}
