using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBar : MonoBehaviour {

	public Image healthFullImage;
    public float maxHealth = 100f;
    public float currentHealth;

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
		healthFullImage.fillAmount = .5f;
	}
	
	// Update is called once per frame
	void Update () {
        healthFullImage.fillAmount = currentHealth / maxHealth;
	}
}
