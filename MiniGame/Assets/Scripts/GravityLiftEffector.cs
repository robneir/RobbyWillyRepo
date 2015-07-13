using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]
public class GravityLiftEffector : MonoBehaviour {


	float maxGravLiftY = 25f;
	float gravLiftBoostSmooth = .1f;
	Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void OnTriggerStay2D(Collider2D c)
	{
		if(c.gameObject.tag == "Sign")
		{
			
		}
		else if(c.gameObject.tag == "GravityLift")
		{
			//do some wacky shit mayne
			if(rb.velocity.y < maxGravLiftY)
			{
				rb.velocity += Vector2.up * Mathf.Lerp(0f, maxGravLiftY, gravLiftBoostSmooth);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
