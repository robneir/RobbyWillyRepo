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
    public PlayerState currPlayerState = PlayerState.OnFoot;

    private Rigidbody2D rigidBody2D;
	private Animator animator;
	private bool OnGround=true;
	private float gravityScale; //needed don't delete
    private GameObject currVehicle;

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

        switch(currPlayerState)
        {
            case PlayerState.OnFoot:

                #region OnFoot

                UpdateBasicMovementInput();

                //Change run speed if sprinting or not
                if (Input.GetButton("Sprint") && !animator.GetBool("Jump"))
                {
                    currentRunSpeed = sprintSpeed;
                }
                else if (!animator.GetBool("Jump"))
                {
                    currentRunSpeed = regSpeed;
                }

                #endregion

                break;
            case PlayerState.InTank:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PlayerOutVehicle();
                }
                break;
            case PlayerState.OnLadder:
                UpdateBasicMovementInput();
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
	{
		if(col.collider.tag=="Ground" && !OnGround)
		{
			OnGround=true;
		}
	}

    void UpdateBasicMovementInput()
    {
        #region Get Basic Movement input
        if (xAxisEnabled)
        {
            deltaPos.x = Input.GetAxis("Horizontal") * currentRunSpeed * Time.deltaTime;
            animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal"))); //Set float in animator to control run animation blend tree //Mathf.Abs(deltaX)
        }
        if (zAxisEnabled)
            deltaPos.z = Input.GetAxis("Vertical") * currentRunSpeed * Time.deltaTime;
        if (yAxisEnabled)
        {
            if (currPlayerState == PlayerState.OnLadder)
            {
                deltaPos.y = Input.GetAxis("Vertical") * climbSpeed * Time.deltaTime;
                rigidBody2D.gravityScale = 0;
            }
            else rigidBody2D.gravityScale = this.gravityScale;

            if (Input.GetButtonDown("Jump") && OnGround)
            {
                rigidBody2D.AddForce(new Vector2(0, this.jumpPower));
                OnGround = false;
            }

            if (!OnGround)
            {
                animator.SetBool("Jump", true);
            }
            else
            {
                animator.SetBool("Jump", false);
            }
        }
        transform.position = deltaPos + transform.position;
        #endregion

        #region Changing directions
        if ((Input.GetAxisRaw("Horizontal") > 0 && this.transform.localScale.x < 0) ||
            ((Input.GetAxisRaw("Horizontal") < 0 && this.transform.localScale.x > 0)))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1,
                                                  this.transform.localScale.y,
                                                  this.transform.localScale.z);
        }
        #endregion

        //Change speed of animation based on speed
        animator.speed = currentRunSpeed / regSpeed;
    }

    public void PlayerInVehicle(GameObject vehicle)
    {
        //tell player he is in tank and set things on and off depending on what needs to be
        currPlayerState = PlayerState.InTank;
        currVehicle = vehicle;
        rigidBody2D.gravityScale = 0;
        rigidBody2D.isKinematic = true;
        this.transform.localScale = new Vector3(1, 1, 1);
        this.GetComponentInChildren<BoxCollider2D>().enabled = false;
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
        if(this.GetComponent<PlayerItems>().Current!=null)
        {
            this.GetComponent<PlayerItems>().Current.SetActive(false);
        }
        transform.SetParent(vehicle.transform);
        transform.localPosition = vehicle.GetComponentInParent<TankMovement>().playerInTankOffset;
        transform.rotation = vehicle.transform.rotation; 
    }

    public void PlayerOutVehicle()//GameObject vehicle
    {
        //tell player he is out of tank and set things on and off depending on what needs to be
        currPlayerState = PlayerState.OnFoot;
        currVehicle.GetComponentInParent<TankMovement>().tankIsManned = false;
        rigidBody2D.gravityScale = gravityScale;
        rigidBody2D.isKinematic = false;
        this.GetComponentInChildren<BoxCollider2D>().enabled = true;
        this.GetComponentInChildren<SpriteRenderer>().enabled = true;
        this.transform.rotation=Quaternion.Euler(0,0,0);
        if (this.GetComponent<PlayerItems>().Current != null)
        {
            this.GetComponent<PlayerItems>().Current.SetActive(true);
        }
        transform.SetParent(null);
        currVehicle = null;
    }
}
