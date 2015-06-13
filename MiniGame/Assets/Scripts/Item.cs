using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	public bool HasUser = false;
	public Transform SetTran;

	public string Name = "";
	public int Damage = 0;
	public int Defense = 0;
	[HideInInspector]
	public UseItem UseFunc;
	public ItemType Type;
	[HideInInspector]
	public Vector3 Offset;

	public enum ItemType
	{
		Sword,
		Dagger,
		Gun,
		BLANK
	}

	// Use this for initialization
	void Start () 
	{
		switch(Type)
		{
			case ItemType.Gun:
			Offset = new Vector3(-2.329968f,-0.8299061f,0);
					break;
			case ItemType.Dagger:
				Offset = new Vector3(10,10,0);
				break;
			case ItemType.Sword:
				Offset = new Vector3(2.329968f,-0.9299061f,0);
				break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(HasUser && !GetComponent<Rigidbody2D>().fixedAngle)
		{
			GetComponent<Rigidbody2D>().fixedAngle = true;
			GetComponent<Transform>().rotation = SetTran.rotation;
			GetComponent<Transform>().localPosition = Offset;
		}
		else
		{
			GetComponent<Rigidbody2D>().fixedAngle = false;
		}
	}

	void ShootRocket()
	{

	}

	public delegate void UseItem();
}
