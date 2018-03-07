using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class CinematicCanvas : MonoBehaviour {

	public TextMeshProUGUI text1;
	private DialogStateNode lastNode;

	public void Start()
	{
		text1.enabled = false;
		TransmissionManager.Instance.OnNodeIn += ShowDialogNodeFeedback;
		TransmissionManager.Instance.OnPathGo += ShowDialogPathFeedback;
		GetComponentInChildren<Person> ().Talk ();
	}

	public void OnDestroy()
	{
		TransmissionManager.Instance.OnNodeIn -= ShowDialogNodeFeedback;
		TransmissionManager.Instance.OnPathGo -= ShowDialogPathFeedback;
	}

	void Update()
	{
		if(Input.anyKeyDown)
		{

				TransmissionManager.Instance.SelectDialogVariant ((DialogStatePath)lastNode.pathes[0]);
		
		}
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
		
	private void ShowDialogNodeFeedback(DialogStateNode node)
	{
		lastNode = node;
		if(node.text!="")
		{
			Show (node.text);
		}
	}

	private void ShowDialogPathFeedback(DialogStatePath path)
	{
		if(path.text!="")
		{
			Show(path.text);
		}
	}


	private void Show(string s)
	{
		text1.text = s;
		text1.enabled = true;
	}

}
