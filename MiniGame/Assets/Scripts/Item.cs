using UnityEngine;
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
	public Vector3 Offset;
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
						Offset = new Vector3(3.469929f,-1.510031f,0);
						UseFunc = FireShotgun;
						break;
				}
				break;
			case ItemType.Melee:
				Offset = new Vector3(2.329968f,-0.9299061f,0);
                UseFunc = SwingSword;
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

	private void FireShotgun()
	{
		InstantiateBullets ();
	}

    private void SwingSword()
    {
        isBeingUsed = true;
    }

	void InstantiateBullets()
	{
		GameObject.Destroy((GameObject)GameObject.Instantiate(MuzzleFlash, this.FireTip.position, Quaternion.identity), 1f);	

		GameObject bull = (GameObject)PhotonNetwork.Instantiate("Bullet", this.FireTip.position, FireTip.rotation, 0);	
		bull.GetComponent<Bullet> ().Damage = this.Damage;
		bull.GetComponent<Rigidbody2D>().velocity = new Vector3(FireTip.right.x * this.transform.root.localScale.x, FireTip.right.y, FireTip.right.z);
		bull.GetComponent<Rigidbody2D>().velocity *= FireSpeed;
	}

	public delegate void UseItem();

	#endregion
}
