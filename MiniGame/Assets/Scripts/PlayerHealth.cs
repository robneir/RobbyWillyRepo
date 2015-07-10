﻿using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerHealth : Photon.MonoBehaviour {

	//Publics
    public Vector2 barOffSet = Vector2.zero;
	public GameObject StatusBar;

    //Privates
    private GameObject statusBar;
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
	}

    void Update()
    {

    }

	public void Die()
	{
        this.GetComponent<PhotonView>().RPC("DestroyStatusBar", PhotonTargets.AllBuffered);
		dead = true;
		this.gameObject.active = false;
	}

    [RPC]
    void AddHealth(int health)
    {
        //Subtract Health and check to see if dead
        if (statusBar != null)
        {
            if(statusBar.GetComponent<StatusBar>().targetHealth+health>= statusBar.GetComponent<StatusBar>().maxHealth)
            {
                statusBar.GetComponent<StatusBar>().targetHealth += statusBar.GetComponent<StatusBar>().maxHealth;
                statusBar.GetComponent<StatusBar>().currentHealth += statusBar.GetComponent<StatusBar>().maxHealth;
            }
            else
            {
                statusBar.GetComponent<StatusBar>().targetHealth += health;
                statusBar.GetComponent<StatusBar>().currentHealth += health;
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
			statusBar = (GameObject)Instantiate(StatusBar,this.transform.position, Quaternion.identity);
			statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
			statusBar.GetComponent<StatusBar>().targetHealth = (float)startHealth;
			statusBar.GetComponent<StatusBar>().currentHealth = (float)startHealth;
		}
    }

    [RPC]
    void DestroyStatusBar()
    {
        GameObject.Destroy(statusBar);
    }
}
