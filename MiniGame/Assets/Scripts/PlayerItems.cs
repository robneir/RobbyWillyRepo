using UnityEngine;
using System.Collections;

public class PlayerItems : MonoBehaviour {

	public GameObject Current = null;

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
			if(Input.GetButtonDown("UseWeapon") && Current.GetComponent<Item>().HasUser)
			{
				//play swing sword animation/fire or whatever
				Current.GetComponent<Item>().UseFunc();
			}

			if(Input.GetKeyDown(KeyCode.Q))
			{
				//throw item
				if(transform.localScale.x == -1)
				{
					Current.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 3);
					Current.GetComponent<Item>().HasUser = false;
					Current.GetComponent<SpriteRenderer>().enabled = false;
				}
				else
				{
					Current.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 3);
					Current.GetComponent<Item>().HasUser = false;
					Current.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		//if youre colliding with an item with no user
		if(c.gameObject.tag.Equals("Item") && !c.gameObject.GetComponent<Item>().HasUser)
		{
			//assign item to user
					//Current = c.gameObject;
					//Current.GetComponent<SpriteRenderer>().enabled = false;
			Current.GetComponent<SpriteRenderer>().sprite = c.gameObject.GetComponent<SpriteRenderer>().sprite;
			Current.GetComponent<SpriteRenderer>().enabled = true;

			//deep copy of item components
			Item i = c.gameObject.GetComponent<Item>();
			Current.GetComponent<Item>().HasUser = true;
			Current.GetComponent<Item>().Damage = i.Damage;
			Current.GetComponent<Item>().Defense = i.Defense;
			Current.GetComponent<Item>().UseFunc = i.UseFunc;
			Current.GetComponent<Item>().SetTran = i.SetTran;
			Current.GetComponent<Item>().Offset = i.Offset;
			Current.GetComponent<Item>().Name = i.Name;
			GameObject.Destroy(c.gameObject);
		}
        else if(c.gameObject.tag.Equals("Item") && c.gameObject.GetComponent<Item>().HasUser)
        {
            Item item = c.gameObject.GetComponent<Item>();
            if(item.isBeingUsed)
            {
                GetComponent<PlayerHealth>().TakeDamage(item.Damage);
            }
        }
	}
}
