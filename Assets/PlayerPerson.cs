using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPerson : MonoBehaviour {

	private string fromScene;

	void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;

		OnSceneLoaded (SceneManager.GetActiveScene(), LoadSceneMode.Single);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		MovementNet net = FindObjectOfType<MovementNet> ();
		GetComponentInChildren<SpriteRenderer> ().enabled = (net != null);
		GetComponentInChildren<PolygonCollider2D> ().enabled = (net != null);
		GetComponent<NetWalker> ().SetNet (net);
		MovePlayer ();
	}

	void OnSceneUnloaded(Scene scene)
	{
		fromScene = scene.name;
	}

	private void MovePlayer()
	{

		if(GetComponent<NetWalker> ().net)
		{
			NetNode node = GetComponent<NetWalker> ().net.GetPointWithName (fromScene);	
			if(node!=null)
			{
				GetComponent<NetWalker> ().transform.position = GetComponent<NetWalker> ().net.GetNodeWorldPosition (node);
			}
		}
	}
}
