using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	//Publics
    public Vector2 barOffSet = Vector2.zero;
	public GameObject StatusBar;

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
		else
		{

		}
    }

    void Update()
    {

    }

	public void Die()
	{
        Destroy(statusBar);
		PhotonNetwork.Destroy (this.gameObject);
	}
    
	[RPC]
	void TakeDamage(int d)
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

	/*void TakeDamage(int d)
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
    }*/

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

	[RPC]
    void InstantiateHealthBar(float startHealth)
    {
		if(startHealth == null)
		{
			statusBar = (GameObject)Instantiate(StatusBar, Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity);
			statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
		}
		else
		{
			statusBar = (GameObject)Instantiate(StatusBar, Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(100,100, 0)), Quaternion.identity);
			statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
			statusBar.GetComponent<StatusBar>().targetHealth = (float)startHealth;
			statusBar.GetComponent<StatusBar>().currentHealth = (float)startHealth;
		}
    }
}
