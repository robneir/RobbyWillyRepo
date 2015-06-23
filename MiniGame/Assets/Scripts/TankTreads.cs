using UnityEngine;
using System.Collections;

public class TankTreads : MonoBehaviour {

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground" && col.gameObject.ToString()!=this.gameObject.ToString())
        {
            transform.root.gameObject.GetComponent<TankMovement>().onGround = true;
        }
    }
}
