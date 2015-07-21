using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    public int value = 25;

    private bool isPickedUp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(isPickedUp)
        {
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.transform.position = Vector3.Lerp(this.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0)), .12f);
            this.transform.Rotate(new Vector3(3, 3, 0));
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag=="Player")
        {
            isPickedUp = true;
            col.gameObject.GetComponent<PlayerStatus>().AddGold(value);
        }else if(isPickedUp && col.name=="TopCornerGoldDestroyer")
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
