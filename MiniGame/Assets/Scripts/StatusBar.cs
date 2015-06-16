using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBar : MonoBehaviour {

	public Image healthFullImage;
    public float maxHealth = 100f;
	[HideInInspector]
    public float currentHealth;
	[HideInInspector]
	public float targetHealth;

    // Use this for initialization
    void Start () {
        currentHealth = 0;
		healthFullImage.fillAmount = .5f;
		targetHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () 
	{
        healthFullImage.fillAmount = currentHealth / maxHealth;
        if(currentHealth!=targetHealth)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, .08f);
        }
	}
}
