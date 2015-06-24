using UnityEngine;
using System.Collections;

public class PointTowardMouse : MonoBehaviour {

    public float recoilOffset;
	bool IsMine;
	IsVehicle IV = null;

	// Use this for initialization
	void Start ()
	{
		if(transform.root.GetComponent<PhotonView>().isMine)
		{
			IsMine = true;
		}
		else if(transform.root.GetComponent<IsVehicle>() != null)
		{
			IsMine = true;
			IV = transform.root.GetComponent<IsVehicle>();
			IV.ScannedForActivePlayer = false;
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
			if(IV != null && IV.OccupiedPlayer != null)
			{
				if(IV.OccupiedPlayer.GetComponent<PhotonView>().isMine)
				{
					IV.ScannedForActivePlayer = true;
				}
				else
				{
					IsMine = false;
					IV.ScannedForActivePlayer = true;
				}
			}
	        //Called in late update to override the animation
			Vector3 mouseDiff = Input.mousePosition - Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y,0));
	        mouseDiff.Normalize();

			if(transform.root.localScale.x > 0)
			{
		        float rotation = Mathf.Atan2(mouseDiff.y, mouseDiff.x) * Mathf.Rad2Deg;
		        Mathf.Clamp(rotation, 0, 90);
				this.transform.rotation = Quaternion.Euler(0,0,rotation+recoilOffset);
			}
			else
			{
				float rotation = Mathf.Atan2(-mouseDiff.y, -mouseDiff.x) * -Mathf.Rad2Deg;
				Mathf.Clamp(rotation, -90, 0);
				this.transform.rotation = Quaternion.Euler(0,0,rotation+recoilOffset);
			}

			recoilOffset = Mathf.Lerp(recoilOffset, 0, .1f);
		}
	}
}
