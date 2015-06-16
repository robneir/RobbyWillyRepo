using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBar : MonoBehaviour {

    //Healh variables
	public Image healthFullImage;
    public float maxHealth = 100f;
	[HideInInspector]
    public float currentHealth;
	[HideInInspector]
	public float targetHealth;

    //Mana variables
    public Image manaFullImage;
    public float maxMana = 100f;
    [HideInInspector]
    public float currentMana;
    [HideInInspector]
    public float targetMana;

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth/2;
		healthFullImage.fillAmount = .5f;
		targetHealth = maxHealth;

        currentMana = maxMana / 2;
        manaFullImage.fillAmount = .5f;
        targetMana = maxMana;
    }
	
	// Update is called once per frame
	void Update () 
	{
        //Update health 
        healthFullImage.fillAmount = currentHealth / maxHealth;
        if(currentHealth!=targetHealth)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, .1f);
        }
        //Update Mana
        manaFullImage.fillAmount = currentMana / maxMana;
        if (currentMana!= targetMana)
        {
            currentMana = Mathf.Lerp(currentMana, targetMana, .1f);
        }
    }
}
