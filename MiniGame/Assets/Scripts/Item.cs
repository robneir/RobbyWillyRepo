﻿using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	#region Visible In Inspector
	/// <summary>
	/// Weather or not the item is being used by a player.
	/// </summary>
	public bool HasUser = false;
	/// <summary>
	/// The name of the item.
	/// </summary>
	public string Name = "";
	/// <summary>
	/// The damage, if any, of the item
	/// </summary>
	public int Damage = 0;
	/// <summary>
	/// The defense, if any, of the item
	/// </summary>
	public int Defense = 0;
	/// <summary>
	/// The ItemType.
	/// </summary>
	public ItemType Type;
	/// <summary>
	/// The transform used to fire out of the tip of an ammo based weapon.
	/// </summary>
	public Transform FireTip;
	/// <summary>
	/// The muzzle flash (usually particle system) to use on an ammo based weapon. Fired out of the FireTip.
	/// </summary>
	public GameObject MuzzleFlash;
	/// <summary>
	/// The fire speed, if any, of a projectile out of this item.
	/// </summary>
	public float FireSpeed = 50;

	#endregion

	#region Hidden From Inspector Variables

	[HideInInspector]
	public UseItem UseFunc;
	[HideInInspector]
	public Vector3 posOffset;

	//to be put into a Quaternion later, using Quaternion.Euler(Vector3 rotationVector) (we can rotation take values from inspector this way.)
    [HideInInspector]
    public Vector3 rotOffset;
    [HideInInspector]
	public Vector3 OriginalScale;
	[HideInInspector]
    public bool isBeingUsed;

	#endregion

	public enum ItemType
	{
		Melee,
		Ammo,
		OneShot,
		Special
	}

	// Use this for initialization
	void Start () 
	{
		OriginalScale = transform.localScale;

		switch(Type)
		{
			case ItemType.Ammo:
				switch(Name)
				{
					case "Shotgun":
						posOffset = new Vector3(0.07998937f,0-1.710141f,0);
						rotOffset = new Vector3(0,0,276.1943f);
				        UseFunc = FireSemiAuto;
						break;
					case "Colt45":
						posOffset = new Vector3(0.6901398f,-2.161226f,0);
						rotOffset = new Vector3(0,0, 267.1234f);
						UseFunc = FireSemiAuto;
						break;
				break;
			}
			break;
			case ItemType.Melee:
				switch(Name)
				{
					case "Katana":
                        posOffset = new Vector3(2.83f, -0.3200116f, 0);
                        //rotOffset = Quaternion.Euler(0, 0, 0);
		                UseFunc = SwingSword;
						break;
				}
				break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if(HasUser && !GetComponent<Rigidbody2D>().fixedAngle)
		{
			GetComponent<Rigidbody2D>().fixedAngle = true;
			GetComponent<Transform>().rotation = SetTran.rotation;
			GetComponent<Transform>().localPosition = Offset;
		}
		else
		{
			GetComponent<Rigidbody2D>().fixedAngle = false;
		}*/
	}

	#region UseItem methods
	//Here will lie all the methods for using unique items. Some may be shared. 
	//EG---> SwingSword() may be applicable to both a baseball bat and a katana
	//EG---> FireRocket() will probably be different than FireHandgun()

	private void FireSemiAuto()
	{
		InstantiateBullet ();
		DoRecoil ();
	}

	void DoRecoil ()
	{
		GameObject rotateArm = this.transform.parent.parent.parent.gameObject;
		rotateArm.transform.GetComponent<PointTowardMouse> ().recoilOffset += 5f;
	}

    private void SwingSword()
    {
        isBeingUsed = true;
    }

	void InstantiateBullet()
	{
		GameObject muz = (GameObject)GameObject.Instantiate (MuzzleFlash, this.FireTip.position, Quaternion.identity);
		GameObject.Destroy(muz, 1f);

		PhotonView pv = this.transform.root.gameObject.GetComponent<PhotonView>();
		if(pv.isMine)
		{
			Vector3 sendV = new Vector3 (FireTip.right.x * this.transform.root.localScale.x, FireTip.right.y, FireTip.right.z) * FireSpeed;
			float lScale = this.transform.root.localScale.x;
			pv.RPC ("FireBullet", PhotonTargets.All, PhotonNetwork.player.ID, FireTip.position, FireTip.rotation, sendV, this.Damage, lScale);
		}
	}

	public delegate void UseItem();

	#endregion
}
