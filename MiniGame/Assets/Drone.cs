using UnityEngine;
using System.Collections;

public class Drone : MonoBehaviour 
{
	[HideInInspector]
	public GameObject Owner = null;
	[HideInInspector]
	public GameObject Target = null;
	
	public GameObject DroneEye;
	float EyeMaxDistanceMagnitude = .5f;
	Vector3 DroneEyeStartPos;
	
	// Use this for initialization
	void Start () 
	{
		DroneEyeStartPos = DroneEye.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 middleDrone = this.transform.position + DroneEyeStartPos;

		if(Target != null)
		{
			//shoot him

			//follow him with your eye
			Vector3 dir = Target.transform.position - DroneEye.transform.position;
			dir.Normalize();
			dir *= EyeMaxDistanceMagnitude;

			DroneEye.transform.position = Vector3.Lerp(DroneEye.transform.position, DroneEyeStartPos + dir, .05f);
		}
		else
		{
			DroneEye.transform.position = Vector3.Lerp(DroneEye.transform.position, DroneEyeStartPos, .05f);
		}
	}

	void Trigger(Collider2D c)
	{
		if (Target != null)
			return;
		
		if(c.gameObject.tag.Equals("Player"))
		{
			Target = c.gameObject;
		}
	}
	
	void OnTriggerEnter2D(Collider2D c)
	{
		Trigger (c);
	}

	void OnTriggerStay2D(Collider2D c)
	{
		Trigger (c);
	}
	
	void OnTriggerExit2D(Collider2D c)
	{
		if(Target != null &&
		   c.gameObject == Target)
		{
			//lose target
			Target = null;
		}
	}
}
