using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPerson : MonoBehaviour {

	private string fromScene;

	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
        GameScenesManager.Instance.OnSaveLoaded += SaveLoaded;
		OnSceneLoaded (SceneManager.GetActiveScene(), LoadSceneMode.Single);
	}
    
    private void SaveLoaded(SaveStruct obj)
    {
        transform.position = obj.playerPosition;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log ("scene loaded");
		PolyNav2D net = FindObjectOfType<PolyNav2D> ();
		GetComponentInChildren<SpriteRenderer> ().enabled = (net != null);
		GetComponentInChildren<PolygonCollider2D> ().enabled = (net != null);

		Debug.Log (net);
		if(net)
		{
			Vector2 point = net.GetComponent<PolygonCollider2D> ().Distance(GetComponentInChildren<PolygonCollider2D>()).pointA;
			transform.position = new Vector3 (point.x, point.y, transform.position.z);
		}
        //MovePlayer();
    }

	void OnSceneUnloaded(Scene scene)
	{
		fromScene = scene.name;   
	}

	/*
	private void MovePlayer()
	{
		if(GetComponent<NetWalker> ().net)
		{
			NetNode node = GetComponent<NetWalker> ().net.GetPointWithName (fromScene);	
			if(node!=null)
			{
				Vector3 point = GetComponent<NetWalker> ().net.GetNodeWorldPosition(node);
				GetComponent<NetWalker> ().Stop(point);
			}
            else
            {
				Vector3 point = GetComponent<NetWalker> ().net.GetNodeWorldPosition (GetComponent<NetWalker> ().net.GetNearestPoint (transform.position));
				GetComponent<NetWalker> ().Stop(point);
			}
		}
	}*/
}
