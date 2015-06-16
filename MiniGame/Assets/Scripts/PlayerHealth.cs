using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	//Publics
    public Vector2 barOffSet = Vector2.zero;

    //Privates
    private GameObject statusBar;
    private GameObject UICanvas;

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
    }

    void Update()
    {
        //JUST FOR TESTING HEALTH AND MANA
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(30);
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            UseEnergy(30);
        }
    }

	public void Die()
	{
        PhotonNetwork.Destroy(statusBar);
		PhotonNetwork.Destroy (this.gameObject);
	}
    
	public void TakeDamage(int d)
	{
        //Subtract Health and check to see if dead
        if (statusBar != null)
        {
            statusBar.GetComponent<StatusBar>().targetHealth -= d;
            statusBar.GetComponent<StatusBar>().currentHealth -= d;
            if (statusBar.GetComponent<StatusBar>().currentHealth <= 0)
            {
                //Die
                Die();
            }
        }
        else
        {
            Debug.LogError("StatusBar is null therefore cannot subtract health from it");
        }
    }

    public void UseEnergy(int mana)
    {
        if (statusBar != null)
        {
            statusBar.GetComponent<StatusBar>().targetMana -= mana;
            statusBar.GetComponent<StatusBar>().currentMana -= mana;
        }
        else
        {
            Debug.LogError("StatusBar is null therefore cannot subtract mana from it");
        }
    }

    public void InstantiateHealthBar()
    {
        statusBar = (GameObject)PhotonNetwork.Instantiate("StatusBar", Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity, 0);
        statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
    }
}
