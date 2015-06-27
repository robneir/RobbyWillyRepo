using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerItems : Photon.MonoBehaviour {

	public GameObject Current = null;
	public GameObject ArmTransform;
	public GameObject ArmNear;
	public GameObject ArmFar;

	public Text pickupUI;
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
		if (pickupUI == null)
			pickupUI =(Text) GameObject.FindGameObjectWithTag("TextUI").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Current != null)
        {
            //you have a item now check what kind of item it is an execute functions base on that
            if (Current.GetComponent<Item>().HasUser && Current.active == true)
            {
                Item i = Current.GetComponent<Item>();
                if (!Current.GetComponent<Item>().isAutomatic &&
                    Input.GetButtonDown("UseWeapon")) //active is set to false when in a vehicle so check to see if in vehicle before shooting
                {
                    #region IF SEMI-AUTO/SWORD
                    //play swing sword animation/fire or whatever
                    i.UseFunc();

                    if (photonView.isMine)
                    {
                        switch (i.Type)
                        {
                            //melee weapons
                            case Item.ItemType.Melee:
                                switch (i.Name)
                                {
                                    case "Katana":
                                        photonView.RPC("SyncTrigger", PhotonTargets.All, "Swing");
                                        break;
                                }
                                break;
                                //end melee
                        }
                    }
                    #endregion
                }
                else if(i.isAutomatic && Input.GetButton("UseWeapon"))
                {
                    #region IF AUTOMATIC
                    if(Time.time- i.lastShotTime>i.timeBetweenShots)
                    {
                        i.lastShotTime = Time.time;
                        i.UseFunc();
                    }
                    #endregion
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


		//i think this is now fixed
		Current.transform.rotation = ArmTransform.transform.rotation;
		Current.transform.localRotation = Quaternion.Euler(Current.GetComponent<Item> ().rotOffset);
    }

	void OnCollisionEnter2D(Collision2D c)
	{
        if(c.gameObject.tag.Equals("Item") && c.gameObject.GetComponent<Item>().HasUser)
        {
            Item item = c.gameObject.GetComponent<Item>();

            if(item.isBeingUsed)
            {
                //Do Damage
				this.photonView.RPC("TakeDamage", PhotonTargets.AllBuffered, item.Damage);
            }
        }
	}

	void OnCollisionStay2D(Collision2D c)
	{
		//if youre colliding with an item with no user
		if(c.gameObject.tag.Equals("Item") && !c.gameObject.GetComponent<Item>().HasUser)
		{
			if(Input.GetButtonDown("Swap"))
			{
				if(Current != null)
					ThrowItem(Current);
				
				AssignItem(c.gameObject);
			}
			if(Current != null)
			{
				pickupUI.text = "Press E to swap for " + c.gameObject.GetComponent<Item>().Name + ".";
			}
			else if(pickupUI!=null)
			{
				pickupUI.text = "Press E to pickup " + c.gameObject.GetComponent<Item>().Name + ".";
			}
            else
            {
                pickupUI = GameObject.FindGameObjectWithTag("TextUI").GetComponent<Text>();
            }
		}
		else if(pickupUI != null) pickupUI.text = "";
	}
}
