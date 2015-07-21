using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerStatus : Photon.MonoBehaviour {

	//Publics
    public Vector2 barOffSet = Vector2.zero;
    public StatusBar statusBar;

    [HideInInspector]
    public int gold = 250;

    //Privates
    private GameObject UICanvas;

	public bool dead = false;

    void FixedUpdate()
    {
        if(statusBar!=null)
        {
            //Update position of bar
            statusBar.transform.position =
            	new Vector3(this.transform.position.x + barOffSet.x,
            	this.transform.position.y + barOffSet.y,
            	this.transform.position.z);
        }
		else
		{

		}
    }

	[RPC]
	void RespawnMe(Vector3 pos, Quaternion rot)
	{
		dead = false;
		this.gameObject.active = true;
		this.transform.position = pos;
		this.transform.rotation = rot;	
		ShowStatusBar ();
	}

    void Update()
    {

    }

    public void AddGold(int gold)
    {
        this.gold += gold;
    }

    public void SubtractGold()
    {
        this.gold -= gold;
    }

	public void Die()
	{
		this.GetComponent<PhotonView>().RPC("HideStatusBar", PhotonTargets.All);
		dead = true;
		this.gameObject.active = false;
	}

    public bool HasMana()
    {
        return statusBar.currentMana > 0;
    }

    [RPC]
    void AddHealth(int health)
    {
        //Subtract Health and check to see if dead
        if (statusBar != null)
        {
            if(statusBar.GetComponent<StatusBar>().targetHealth+health>= statusBar.GetComponent<StatusBar>().maxHealth)
            {
                statusBar.GetComponent<StatusBar>().targetHealth = statusBar.GetComponent<StatusBar>().maxHealth;
                statusBar.GetComponent<StatusBar>().currentHealth = statusBar.GetComponent<StatusBar>().maxHealth;
            }
            else
            {
                statusBar.GetComponent<StatusBar>().targetHealth += health;
                statusBar.GetComponent<StatusBar>().currentHealth += health;
            }
        }
    }

    [RPC]
    void AddMana(int mana)
    {
        //Subtract Health and check to see if dead
        if (statusBar != null)
        {
            if (statusBar.GetComponent<StatusBar>().targetMana + mana >= statusBar.GetComponent<StatusBar>().maxMana)
            {
                statusBar.GetComponent<StatusBar>().targetMana = statusBar.GetComponent<StatusBar>().maxMana;
                statusBar.GetComponent<StatusBar>().currentMana = statusBar.GetComponent<StatusBar>().maxMana;
            }
            else
            {
                statusBar.GetComponent<StatusBar>().targetMana += mana;
                statusBar.GetComponent<StatusBar>().currentMana += mana;
            }
        }
    }

    [RPC]
    void TakeMana(float mana)
    {
        //Subtract Health and check to see if dead
        if (statusBar != null)
        {
            if (statusBar.GetComponent<StatusBar>().targetMana - mana <= 0)
            {
                statusBar.GetComponent<StatusBar>().targetMana = 0;
                statusBar.GetComponent<StatusBar>().currentMana = 0;
            }
            else
            {
                statusBar.GetComponent<StatusBar>().targetMana -= mana;
                statusBar.GetComponent<StatusBar>().currentMana -= mana;
            }
        }
    }

    [RPC]
	void TakeDamage(int d, int dealerID, int deathID)
	{
		Debug.Log (dealerID + " just injured " + deathID);
        //Subtract Health and check to see if dead
        if (statusBar != null)
        {
            statusBar.GetComponent<StatusBar>().targetHealth -= d;
            statusBar.GetComponent<StatusBar>().currentHealth -= d;

            if (statusBar.GetComponent<StatusBar>().currentHealth <= 0)
            {
                //Die
				//this.GetComponent<Scorecard>().PlayerKilled(dealerID, this.GetComponent<PhotonView>().ownerId);
				//this.GetComponent<PhotonView>().RPC ("PlayerKilled", PhotonTargets.All, dealerID, this.GetComponent<PhotonView>().ownerId);

				if(PhotonNetwork.player.ID == dealerID)
				{
					//you are dope you killed him
					//PhotonHashTable pht = new PhotonHashTable();
					PhotonNetwork.player.customProperties["Kills"] = (int)PhotonNetwork.player.customProperties["Kills"] + 1;
					PhotonNetwork.player.customProperties["Deaths"] = (int)PhotonNetwork.player.customProperties["Deaths"];
					PhotonNetwork.player.customProperties["Assists"] = (int)PhotonNetwork.player.customProperties["Assists"];
					PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.customProperties);
				}
				else if(PhotonNetwork.player.ID == deathID)
				{
					//you suck you are dead
					//PhotonHashTable pht = new PhotonHashTable();
					PhotonNetwork.player.customProperties["Kills"] = (int)PhotonNetwork.player.customProperties["Kills"];
					PhotonNetwork.player.customProperties["Deaths"] = (int)PhotonNetwork.player.customProperties["Deaths"] + 1;
					PhotonNetwork.player.customProperties["Assists"] = (int)PhotonNetwork.player.customProperties["Assists"];
					PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.customProperties);
				}
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

	[RPC]
    void InstantiateHealthBar(float startHealth)
    {
		if(startHealth == null)
		{
			statusBar = (StatusBar)Instantiate(statusBar, Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity);
			statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
		}
		else
		{
			statusBar = (StatusBar)Instantiate(statusBar, this.transform.position, Quaternion.identity);
			statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
			statusBar.GetComponent<StatusBar>().targetHealth = (float)startHealth;
			statusBar.GetComponent<StatusBar>().currentHealth = (float)startHealth;
		}
    }

	[RPC]
	void HideStatusBar()
	{
		statusBar.gameObject.active = false;
	}

	void ShowStatusBar()
	{
		statusBar.gameObject.active = true;
		statusBar.targetHealth = statusBar.maxHealth;
		statusBar.targetMana = statusBar.maxMana;
	}

    [RPC]
    void DestroyStatusBar()
    {
        //GameObject.Destroy(statusBar);
    }
}
