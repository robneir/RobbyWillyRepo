using UnityEngine;
using System.Collections;

public class PointTowardMouse : MonoBehaviour {

    public float zOffSet;
	bool IsMine;

	// Use this for initialization
	void Start ()
	{
		if(transform.root.GetComponent<PhotonView>().isMine)
		{
			IsMine = true;
		}
		else
		{
			IsMine = false;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if(IsMine)
		{
	        //Called in late update to override the animation
			Vector3 mouseDiff = Input.mousePosition - Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y,0));
	        mouseDiff.Normalize();

			if(transform.root.localScale.x > 0)
			{
		        float rotation = Mathf.Atan2(mouseDiff.y, mouseDiff.x) * Mathf.Rad2Deg;
		        Mathf.Clamp(rotation, 0, 90);
		        this.transform.localRotation = Quaternion.Euler(0,0,rotation+zOffSet);
			}
			else
			{
				float rotation = Mathf.Atan2(-mouseDiff.y, -mouseDiff.x) * -Mathf.Rad2Deg;
				Mathf.Clamp(rotation, -90, 0);
				this.transform.localRotation = Quaternion.Euler(0,0,rotation+zOffSet);
			}
		}
	}
}
