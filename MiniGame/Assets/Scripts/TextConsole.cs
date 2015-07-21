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
		m.transform.SetParent (GameObject.Find ("Canvas").transform);
		m.GetComponent<Text> ().text = text;	
		MessageTimes.Add (Time.time);
		Vector3 correctV = UpdateCorrectPosition (Messages.Count);
		MessagePositions.Add (correctV);
		Messages.Add (m);
		UpdateCorrectPosition (Messages.Count);
	}

	void OnGUI()
	{
		PrintToScreen ();
	}

	void PrintToScreen()
	{
		string build = "";
		foreach (var f in Messages)
		{
			build += f.text + "" + "\n";
		}
		GUI.Label (new Rect(0,0,800,800),build);
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
				Debug.Log(MessagePositions[i]);
				Messages[i].transform.position = Vector3.Lerp(Messages[i].transform.position, MessagePositions[i], .13f);
			}
			catch(Exception e)
			{
				Debug.Log(e.Message);
			}

			if(i >= msgNumberCutoff || (Time.time % 60) - MessageTimes[i] > delTime)
			{
				Color c = new Color(Messages[i].material.color.r, 
				                    Messages[i].material.color.g,
				                    Messages[i].material.color.b,
				                    Messages[i].material.color.a - .06f);

				Messages[i].material.color = c;
			}
			else if(i > msgNumberCutoff )
			{
				GameObject.Destroy(Messages[i]);
				Messages.RemoveAt(i);
			}
		}
	}
}

