using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour {
    ParticleSystem part;
	// Use this for initialization
	void Start () {
        part = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(!part.IsAlive())
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
