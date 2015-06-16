using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	//Publics
    public Vector2 barOffSet = Vector2.zero;

    //Privates
    private GameObject statusBar;

    void FixedUpdate()
    {
        if(statusBar!=null)
        {
            //Update position of bar
            statusBar.transform.position = Camera.main.WorldToScreenPoint(
                new Vector3(this.transform.position.x + barOffSet.x,
                this.transform.position.y + barOffSet.y,
                this.transform.position.z));
        }
        //TESTING FOR DAMAGE
        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("TAKE 30 damage");
            GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 30);
        }
    }

	public void Die()
	{
		PhotonNetwork.Destroy (this.gameObject);
	}

    [RPC]
	public void TakeDamage(int d)
	{
        statusBar.GetComponent<StatusBar>().targetHealth -= d;
        if (statusBar.GetComponent<StatusBar>().currentHealth <= 0)
		{
			//die
			Die ();
		}
	}

    public void InstantiateHealthBar()
    {
        statusBar = (GameObject)PhotonNetwork.Instantiate("StatusBar", Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity, 0);
        statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
    }
}
