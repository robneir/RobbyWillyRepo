using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	//Publics
    public Vector2 offSet = Vector2.zero;

    //Privates
    private GameObject statusBar;

    // Use this for initialization
    void Start () {
    }

    void FixedUpdate()
    {
        if(statusBar!=null)
        {
            //Update position of bar
            statusBar.transform.position = Camera.main.WorldToScreenPoint(
                new Vector3(this.transform.position.x + offSet.x,
                this.transform.position.y + offSet.y,
                this.transform.position.z));
        }
    }

	public void Die()
	{
		PhotonNetwork.Destroy (this.gameObject);
	}

	public void TakeDamage(int d)
	{
		if(statusBar.GetComponent<StatusBar>().currentHealth <= 0)
		{
			//die
			Die ();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void InstantiateHealthBar()
    {
        statusBar = (GameObject)PhotonNetwork.Instantiate("StatusBar", Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity, 0);
        statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
    }
}
