using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject customizationPanel;

    private Animator customizationPanelAnimator;

	// Use this for initialization
	void Start () {
        customizationPanelAnimator = customizationPanel.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.I))
        {
            customizationPanelAnimator.SetTrigger("Slide_In");
        }
	}
}
