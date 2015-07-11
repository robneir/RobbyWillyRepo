using UnityEngine;
using System.Collections;

public class LookAtObject : MonoBehaviour {

    public GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target != null)
		{
	        Vector3 diff = target.transform.position - this.transform.position;
	        diff.Normalize();
	        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
    }
}
