using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject loadoutPanel;

    private Animator loadoutPanelAnimator;

	// Use this for initialization
	void Start () {
        loadoutPanelAnimator = loadoutPanel.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.I))
        {
            loadoutPanelAnimator.SetTrigger("Slide_In");
        }
	}
}
