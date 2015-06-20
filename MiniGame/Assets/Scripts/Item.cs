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
	[HideInInspector]
	public Vector3 OriginalScale;
	public Transform FireTip;
	[HideInInspector]
    public bool isBeingUsed;
	public GameObject MuzzleFlash;

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
						Offset = new Vector3(-2.329968f,-0.8299061f,0);
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

	private void FireShotgun()
	{
		InstantiateHBullet ();
	}

    private void SwingSword()
    {
        isBeingUsed = true;
    }

	void InstantiateHBullet()
	{
		GameObject.Destroy((GameObject)GameObject.Instantiate(MuzzleFlash, this.FireTip.position, Quaternion.identity), 1f);	

		for(int i = 0; i < 3; ++i)
		{
			GameObject bull = (GameObject)PhotonNetwork.Instantiate("Bullet", this.FireTip.position, Quaternion.identity, 0);	
			bull.GetComponent<Bullet> ().Damage = this.Damage;
			bull.GetComponent<Rigidbody2D>().velocity =  new Vector3(transform.root.localScale.x * 45, (i - 1) * 4, 0);
		}
	}

	public delegate void UseItem();
}
