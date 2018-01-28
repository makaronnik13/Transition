using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using HoloToolkit.Unity;

public class StartVisualiser : Singleton <StartVisualiser>
{
	public Text text;
	public Image endImage;
	private Button button;
	private bool ending = false;
	public string[] startTexts;

	public string[] endTexts1;
	public string[] endTexts2;
	public string[] endTexts3;
	public string[] endTexts4;

	public Sprite[] endImages;

	private Queue<string> startQueue = new Queue<string>();

	public GameObject[] gameContent;

	// Use this for initialization
	void Start () 
	{
		button = GetComponent<Button> ();
		button.onClick.AddListener (PushTheButton);
		button.enabled = true;
		text.enabled = true;
		GetComponent<Image> ().enabled = true;

		foreach(string s in startTexts)
		{
			startQueue.Enqueue (s);
		}
			
		text.text = startQueue.Dequeue ();

		foreach(GameObject go in gameContent)
		{
			go.SetActive (false);
		}
	}
		

	public void ShowEnd(int id)
	{
		ending = true;
        startQueue.Clear();
        Debug.Log("End "+id);
		switch(id)
		{
		case 0:
			foreach(string s in endTexts1)
			{
				startQueue.Enqueue (s);
			}
			break;
		case 1:
			foreach(string s in endTexts2)
			{
				startQueue.Enqueue (s);
			}
			break;
		case 2:
			foreach(string s in endTexts3)
			{
				startQueue.Enqueue (s);
			}
			break;
		case 3:
			foreach(string s in endTexts4)
			{
				startQueue.Enqueue (s);
			}
			break;
		}

		text.text = startQueue.Dequeue ();
		button.enabled = true;
		text.enabled = true;
		GetComponent<Image> ().enabled = true;
	}

	private void PushTheButton()
	{
		if (startQueue.Count > 0) 
		{
			text.text = startQueue.Dequeue ();
		} 
		else 
		{
			if(ending)
			{
				Application.Quit ();
			}

			Hide ();
			foreach(GameObject go in gameContent)
			{
				go.SetActive (true);
			}
			TransmissionManager.Instance.StartGame ();
		}
	}

	private void Hide()
	{
		button.enabled = false;
		text.enabled = false;
		GetComponent<Image> ().enabled = false;	
	}
}
