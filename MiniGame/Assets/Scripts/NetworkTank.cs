using UnityEngine;
using System.Collections;
using System;

public class NetworkTank : Photon.MonoBehaviour {

    private Transform realBodyTransform;
    private Transform realMainTurretTransform;
    private Transform realSmallTurretTransform;

    public Transform mainTurret;
    public Transform smallTurret;
    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        realBodyTransform = this.transform;
        realMainTurretTransform = this.mainTurret;
        realSmallTurretTransform = this.smallTurret;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            //do nothing
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, realBodyTransform.position, .2f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, realBodyTransform.rotation, .2f);
            mainTurret.rotation = Quaternion.Lerp(mainTurret.rotation, realMainTurretTransform.rotation, .2f);
            smallTurret.rotation = Quaternion.Lerp(mainTurret.rotation, realSmallTurretTransform.rotation, .2f);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            try
            {
                //this is our player. we need to send our actual position to the network
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(mainTurret.rotation);
                stream.SendNext(smallTurret.rotation);
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Failed to send data");
            }
        }
        else
        {
            try
            {
                //this is someone elses player. We need to recieve their position and update our version of that player
                realBodyTransform.position = (Vector3)stream.ReceiveNext();
                realBodyTransform.rotation = (Quaternion)stream.ReceiveNext();
                realMainTurretTransform.rotation = (Quaternion)stream.ReceiveNext();
                realSmallTurretTransform.rotation = (Quaternion)stream.ReceiveNext();
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Failed to recieve data");
            }
        }
    }
}
