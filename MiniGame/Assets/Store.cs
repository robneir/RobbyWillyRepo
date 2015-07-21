using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {

    public GameObject storeUIPanel;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag=="Player")
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (storeUIPanel.gameObject.GetActive() == false)
                    storeUIPanel.SetActive(true);
                else
                    storeUIPanel.SetActive(false);
            }
        }
    }
}
