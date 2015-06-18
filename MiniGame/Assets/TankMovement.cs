using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour {

    public bool xAxisEnabled = false;
    public bool yAxisEnabled = false;
    public bool zAxisEnabled = false;
    public float speed;

    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private Vector3 deltaPos;

    // Use this for initialization
    void Start () {
        rigidBody2D = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (xAxisEnabled)
        {
            deltaPos.x = Input.GetAxis("Horizontal") * speed;
            rigidBody2D.AddForce(new Vector3(Input.GetAxis("Horizontal") * speed,0,0));
        }
        animator.SetFloat("Speed",Mathf.Abs(Input.GetAxis("Horizontal")));
        this.transform.position += deltaPos;
    }
}
