using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPerson : MonoBehaviour {

	void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		OnSceneLoaded (SceneManager.GetActiveScene(), LoadSceneMode.Single);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		MovementNet net = FindObjectOfType<MovementNet> ();
		GetComponentInChildren<SpriteRenderer> ().enabled = (net != null);
		GetComponentInChildren<PolygonCollider2D> ().enabled = (net != null);
		GetComponent<NetWalker> ().SetNet (net);

	}
}
