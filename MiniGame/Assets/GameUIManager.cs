using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GameUIManager : MonoBehaviour {

    public GameObject victoryPointStar;

    private VictoryPoint[] victoryPoints;
    private List<GameObject> victoryPointsUI= new List<GameObject>();

	// Use this for initialization
	void Start () {
        victoryPoints = GameObject.FindObjectsOfType<VictoryPoint>();
        for(int i=0;i<victoryPoints.Length;i++)
        {
            GameObject vP= GameObject.Instantiate(victoryPointStar);
            vP.transform.SetParent(this.gameObject.transform,false);
            vP.transform.localPosition = new Vector2(Screen.width/24-Screen.width/2+i*60,Screen.height/2- Screen.width / 24);
            victoryPointsUI.Add(vP);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    for(int i=0;i<victoryPointsUI.Count;i++)
        {
            victoryPointsUI[victoryPoints[i].victoryPointNumber].transform.FindChild("Fill").GetComponent<Image>().fillAmount = victoryPoints[i].captureBar.fillAmount;
        }
	}
}
