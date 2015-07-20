using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VictoryPoint : MonoBehaviour {

    public Image captureBar;
    public float captureRate;
    public float recoveryRate;
    public int victoryPointNumber;

    private bool beingCaptured;
    private bool isCaptured;
    
    [HideInInspector]
    public GameObject owner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(beingCaptured && !isCaptured)
        {
            captureBar.fillAmount += captureRate;
            if(captureBar.fillAmount>=1)
            {
                isCaptured = true;
            }
        }else if(!isCaptured)
        {
            captureBar.fillAmount -= recoveryRate;
        }

	}

    void OnTriggerStay2D(Collider2D c)
    {
        if(c.tag=="Player")
        {
            beingCaptured = true;
            owner = c.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            beingCaptured = false;
        }
    }
}
