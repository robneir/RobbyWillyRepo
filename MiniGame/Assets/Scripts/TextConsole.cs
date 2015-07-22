using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class TextConsole : MonoBehaviour 
{
	//the list of text messages currently on the console.
	public List<Text> Messages = new List<Text>();
	//the list of timers associated with Messages
	public List<Vector3> MessagePositions= new List<Vector3>();
	//the list of times
	public List<float> MessageTimes = new List<float>();
	//how many to display of the most recent messages
	public int msgNumberCutoff = 4;
	//the time it takes for a message to fade out
	public int fadeTimer = 60 * 4;
	//this Text component
	Text t;
	//gameobject to spawn
	public Text TextObject;

	private float delTime = 4;
	private string endline = "\n";

	public void AddMessage(string text)
	{
		Text m = (Text)GameObject.Instantiate(TextObject, this.transform.position, Quaternion.identity);
		m.transform.SetParent (this.transform, false);
		m.GetComponent<Text> ().text = text;	
		MessageTimes.Add (Time.time);
		Vector3 correctV = UpdateCorrectPosition (Messages.Count);
		MessagePositions.Add (new Vector3(0,Messages.Count,0));
		Messages.Add (m);
	}

	Vector3 UpdateCorrectPosition(int mLength)
	{
		int cutoff = mLength - msgNumberCutoff;
		for (int i = Messages.Count; i > 0; --i) 
		{
			int zeroBased = Messages.Count - i;	

			if(i > cutoff)
			{
				//display
				if(Messages.Count > 0)
				{
					MessagePositions[zeroBased] = this.transform.position + new Vector3(0, zeroBased * 30, 0);
				}
			}
			else 
			{
				Messages[zeroBased].text = "";
			}
		}
		return this.transform.position;
	}

	// Use this for initialization
	void Start () 
	{
		t = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

		for(int i = 0; i < Messages.Count; ++i)
		{
			try
			{
				Messages[i].transform.position = Vector3.Lerp(Messages[i].transform.position,
				                                              this.transform.position + MessagePositions[i], .1f);
			}
			catch(Exception e)
			{
				Debug.Log(e.Message);
			}

			if(i >= msgNumberCutoff || (Time.time % 60) - MessageTimes[i] > delTime)
			{
				//fade out this dumb ass shit
				Messages[i].CrossFadeAlpha(0f,3f,false);
			}
			else if(i > msgNumberCutoff )
			{
				GameObject.Destroy(Messages[i]);
				Messages.RemoveAt(i);
			}
		}
	}
}

