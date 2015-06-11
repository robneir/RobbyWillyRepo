using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	public bool HasUser = false;
	public Transform SetTran;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(HasUser && !GetComponent<Rigidbody2D>().fixedAngle)
		{
			GetComponent<Rigidbody2D>().fixedAngle = true;
			GetComponent<Transform>().rotation = SetTran.rotation;
		}
		else
		{
			GetComponent<Rigidbody2D>().fixedAngle = true;
		}
	}
}
