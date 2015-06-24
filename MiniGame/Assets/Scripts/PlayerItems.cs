using UnityEngine;
using System.Collections;

public class PlayerItems : Photon.MonoBehaviour {

	public GameObject Current = null;
	public GameObject ArmTransform;
	public GameObject ArmNear;
	public GameObject ArmFar;

	public GameObject bulletPrefab;

	[RPC]
	void SyncTrigger(string trigName)
	{
		this.GetComponentInChildren<Animator> ().SetTrigger (trigName);
	}

	[RPC]
	void FireBullet(int playerID, Vector3 position, Quaternion rotation, Vector3 velocity, int damage, float scale)
	{
		GameObject bull = (GameObject)Instantiate (bulletPrefab, position, rotation);	
		bull.GetComponent<Bullet> ().Damage = damage;
		bull.GetComponent<Rigidbody2D> ().velocity = velocity;
			
		if (scale < 0) 
		{
			//change rotation. switch Quaternion's z and w values.
			float z = rotation.z;
			float w = rotation.w;
			bull.transform.rotation = new Quaternion (0, 0, w, z);
		}

	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Current != null)
		{
			//you have a item
			if(Input.GetButtonDown("UseWeapon") && Current.GetComponent<Item>().HasUser && Current.active==true) //active is set to false when in a vehicle so check to see if in vehicle before shooting
			{
				Item i = Current.GetComponent<Item>();
				//play swing sword animation/fire or whatever
				i.UseFunc();

				if(photonView.isMine)
				{
					switch(i.Type)
					{
						//melee weapons
						case Item.ItemType.Melee:
							switch(i.Name)
							{
								case "Katana":
								photonView.RPC("SyncTrigger", PhotonTargets.All, "Swing");
								break;
							}
							break;
						//end melee
					}
				}
			}

			if(Input.GetKeyDown(KeyCode.Q))
			{
				ThrowItem(Current);
			}
		}
	}

	void ThrowItem(GameObject item)
	{
		//throw item
		if(transform.localScale.x == -1)
		{
			item.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 3 + Vector3.right * 3);
		}
		else
		{
			item.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 3 + Vector3.right * -3);
		}
		
		item.GetComponent<Rigidbody2D>().isKinematic = false;
		item.GetComponent<Collider2D>().isTrigger = false;
		item.GetComponent<Item>().HasUser = false;
		item.GetComponent<Rigidbody2D>().fixedAngle = false;
		item.transform.SetParent(null);
		item.transform.localScale = item.GetComponent<Item> ().OriginalScale;
		item = null;
	}

	void AssignItem(GameObject item)
	{
		Current = item;
		Current.transform.SetParent(ArmTransform.transform);
		Current.transform.localScale = Current.GetComponent<Item> ().OriginalScale;
		Current.GetComponent<Rigidbody2D>().isKinematic = true;
		Current.GetComponent<Collider2D>().isTrigger = true;
		Current.GetComponent<Item>().HasUser = true;
		Current.GetComponent<Rigidbody2D>().fixedAngle = true;
		Current.GetComponent<Transform>().rotation = /*Current.GetComponent<Item>().SetTran.rotation +*/ ArmNear.transform.rotation;
		Current.transform.localPosition = Current.GetComponent<Item>().posOffset;
        Current.transform.rotation = Current.GetComponent<Item>().rotOffset;
    }

	void OnCollisionEnter2D(Collision2D c)
	{
		//if youre colliding with an item with no user
		if(c.gameObject.tag.Equals("Item") && !c.gameObject.GetComponent<Item>().HasUser)
		{
			if(Current != null)
				ThrowItem(Current);

			AssignItem(c.gameObject);
		}
        else if(c.gameObject.tag.Equals("Item") && c.gameObject.GetComponent<Item>().HasUser)
        {
            Item item = c.gameObject.GetComponent<Item>();

            if(item.isBeingUsed)
            {
                //Do Damage
				this.photonView.RPC("TakeDamage", PhotonTargets.AllBuffered, item.Damage);
            }
        }
	}
}
