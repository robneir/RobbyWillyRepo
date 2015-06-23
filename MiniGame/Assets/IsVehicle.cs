using UnityEngine;
using System.Collections;

public class IsVehicle : MonoBehaviour {

	public GameObject OccupiedPlayer;
	public bool ScannedForActivePlayer = false;
	// Use this for initialization
	public void OccupiedPlayerLeft()
	{
		ScannedForActivePlayer = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
