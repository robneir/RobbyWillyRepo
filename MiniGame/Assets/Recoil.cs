using UnityEngine;
using System.Collections;

public class Recoil : MonoBehaviour {
    
    private float recoilRotOffset;

    private Vector2 recoilPosOffset;
    private float recoilPosMagnitude;

    public float recoilRotAmount;
    public float recoilPosAmount;
    public float recoilPosClamp;
    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {

        Mathf.Clamp(recoilPosMagnitude, 0, recoilPosClamp);
        Quaternion rotation2 = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
        Vector2 magnitudeVector = new Vector2(Mathf.Cos(rotation2.z), Mathf.Sin(rotation2.z));
        magnitudeVector.Normalize();
        recoilPosOffset = new Vector2(magnitudeVector.x * recoilPosMagnitude, magnitudeVector.y * recoilPosMagnitude);
        
        //recoil
        if (transform.root.localScale.x > 0)
        {
            float rotation = this.transform.rotation.eulerAngles.z;
            Mathf.Clamp(rotation, 0, 90);
            this.transform.rotation = Quaternion.Euler(0, 0, rotation + recoilRotOffset);
            this.transform.localPosition = new Vector3(-recoilPosOffset.x,
             -recoilPosOffset.y, this.transform.localPosition.z);
        }
        else
        {
            float rotation = this.transform.rotation.eulerAngles.z;
            Mathf.Clamp(rotation, -90, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, rotation + recoilRotOffset);
            this.transform.localPosition = new Vector3(-recoilPosOffset.x,
            -recoilPosOffset.y, this.transform.localPosition.z);
        }
        recoilRotOffset = Mathf.Lerp(recoilRotOffset, 0, .1f);
        recoilPosMagnitude = Mathf.Lerp(recoilPosMagnitude, 0, .1f);
    }

    public void DoRecoil()
    {
        recoilRotOffset += recoilRotAmount;
        recoilPosMagnitude += recoilPosAmount;
    }
}
