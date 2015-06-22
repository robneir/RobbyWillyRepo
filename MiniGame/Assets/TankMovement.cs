using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour {

    public bool xAxisEnabled = false;
    public bool yAxisEnabled = false;
    public bool zAxisEnabled = false;
    public float speed;
    public Vector3 playerInTankOffset= Vector3.zero;

    [HideInInspector]
    public bool tankIsManned;

    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private TankShoot tankShootScript;
    private Vector3 deltaPos;
    private bool characterCanManTank;

    // Use this for initialization
    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tankShootScript = GetComponent<TankShoot>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If tank is manned make movement possible
        if(tankIsManned)
        {
            animator.enabled = true;
            tankShootScript.enabled = true;
            tankShootScript.mainCannon.GetComponent<PointTowardMouse>().enabled = true;
            tankShootScript.smallCannon.GetComponent<PointTowardMouse>().enabled = true;
            if (xAxisEnabled)
            {
                deltaPos.x = Input.GetAxis("Horizontal") * speed;
                rigidBody2D.AddForce(new Vector3(Input.GetAxis("Horizontal") * speed, 0, 0));
            }
            animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
            this.transform.position += deltaPos;
        }
        else
        {
            animator.enabled = false;
            tankShootScript.enabled = false;
            tankShootScript.mainCannon.GetComponent<PointTowardMouse>().enabled = false;
            tankShootScript.smallCannon.GetComponent<PointTowardMouse>().enabled = false;
        }
    }

    void NewDriver(GameObject player)
    {
        tankIsManned = true;
        player.GetComponentInParent<PlayerMovement>().PlayerInVehicle(transform.GetChild(0).gameObject);
    }

    void OnGUI()
    {
        //Prompts player to get into tank
        if (characterCanManTank && !tankIsManned)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Label(new Rect(20, 20, 200, 200), "E to get in");
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (col.gameObject.GetComponentInParent<PlayerMovement>().currPlayerState != PlayerState.InTank)
                {
                    NewDriver(col.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="Player")
        {
            characterCanManTank = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            characterCanManTank = false;
        }
    }
}
