using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBar : MonoBehaviour {

	public Image healthFullImage;

	// Use this for initialization
	void Start () {
		healthFullImage.fillAmount = .5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
