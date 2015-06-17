using UnityEngine;
using System.Collections;

public class PlayerMovement : Photon.MonoBehaviour {

	public bool xAxisEnabled=false;
	public bool yAxisEnabled=false;
	public bool zAxisEnabled=false;

	public float regSpeed;
    public float sprintSpeed;
	public float friction;
	public float jumpPower;
	public float climbSpeed;
    public float currentRunSpeed;

	//hidden
	[HideInInspector]
	public Vector3 deltaPos;
	[HideInInspector]
	public PlayerAnimationState aniState = PlayerAnimationState.Idle;

    private Rigidbody2D rigidBody2D;
	private Animator animator;
	private bool OnGround=true;
	[HideInInspector]
	public bool isOnLadder = false;
	private float gravityScale;

	// Use this for initialization
	void Start () {
		rigidBody2D = this.GetComponent<Rigidbody2D> ();
		animator = this.GetComponentInChildren<Animator> ();
		gravityScale = rigidBody2D.gravityScale;
        currentRunSpeed = regSpeed;
	}

	void OnGUI()
	{

	}
	
	// Update is called once per frame
	void Update () {

        #region Get Basic Movement input
		if(xAxisEnabled)
		{
			deltaPos.x =Input.GetAxis ("Horizontal") * currentRunSpeed;
			animator.SetFloat ("Speed", Mathf.Abs(Input.GetAxis("Horizontal"))); //Set float in animator to control run animation blend tree //Mathf.Abs(deltaX)
		}
        if (zAxisEnabled)
			deltaPos.z=Input.GetAxis ("Vertical") * currentRunSpeed;
		if (yAxisEnabled) {
			if(isOnLadder)
			{
				deltaPos.y=Input.GetAxis ("Vertical") * climbSpeed;
				rigidBody2D.gravityScale=0;
			}else rigidBody2D.gravityScale=this.gravityScale;

			if(Input.GetButtonDown("Jump") && OnGround)
			{
				rigidBody2D.AddForce(new Vector2(0,this.jumpPower));
				animator.SetBool("Jump",true);
				OnGround=false;
			}
		}
		transform.position = deltaPos + transform.position;
        #endregion

        #region Changing directions
        if ((Input.GetAxisRaw ("Horizontal")>0 && this.transform.localScale.x<0)||
		    ((Input.GetAxisRaw ("Horizontal")<0 && this.transform.localScale.x>0))) 
		{
			this.transform.localScale=new Vector3(this.transform.localScale.x*-1,
			                                      this.transform.localScale.y,
			                                      this.transform.localScale.z);
		}
        #endregion

        //Change run speed if sprinting or not
        if (Input.GetButton("Sprint") && !animator.GetBool("Jump"))
        {
            currentRunSpeed = sprintSpeed;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            currentRunSpeed = regSpeed;
        }
        //Change speed of animation based on speed
        animator.speed = currentRunSpeed / regSpeed;
    }

    void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.tag=="Ground" && !OnGround)
		{
			OnGround=true;
			animator.SetBool("Jump",false);
		}
	}
}
