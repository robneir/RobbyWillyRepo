using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public Transform[] backgrounds;
	private float[] pScales;
	public float smoothing;

	private Transform cam;
	private Vector3 previousCamPosition;

	void Awake()
	{
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () 
	{
		previousCamPosition = cam.position;

		pScales = new float[backgrounds.Length];

		for(int i = 0; i < backgrounds.Length; ++i)
		{
			pScales[i] = backgrounds[i].position.z *-1;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < backgrounds.Length; ++i)
		{
			float parallax = (previousCamPosition.x - cam.position.x) * pScales[i];
			float bgTargetX = backgrounds[i].position.x + parallax;
			Vector3 bgTarget = new Vector3(bgTargetX, backgrounds[i].position.y, backgrounds[i].position.z);
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, bgTarget, smoothing * Time.deltaTime);
		}

		previousCamPosition = cam.position;
	}
}
