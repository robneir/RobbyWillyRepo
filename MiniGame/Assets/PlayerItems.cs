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
			if(Input.GetButtonDown("Fire1"))
			{
				//play swing sword animation/fire or whatever
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
			Current.GetComponent<Item>().HasUser = true;
			GameObject.Destroy(c.gameObject);
		}
	}
}
