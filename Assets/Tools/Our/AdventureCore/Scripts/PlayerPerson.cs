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
		MovementNet net = FindObjectOfType<MovementNet> ();
		GetComponentInChildren<SpriteRenderer> ().enabled = (net != null);
		GetComponentInChildren<PolygonCollider2D> ().enabled = (net != null);
		GetComponent<NetWalker> ().SetNet (net);
        MovePlayer();
    }

	void OnSceneUnloaded(Scene scene)
	{
		fromScene = scene.name;   
	}

	private void MovePlayer()
	{
		if(GetComponent<NetWalker> ().net)
		{
			Debug.Log (GetComponent<NetWalker> ().net.gameObject);


			NetNode node = GetComponent<NetWalker> ().net.GetPointWithName (fromScene);	
			if(node!=null)
			{
				GetComponent<NetWalker> ().StopCoroutine("MoveFromTo");
				GetComponent<NetWalker>().transform.position = GetComponent<NetWalker> ().net.GetNodeWorldPosition(node);
				GetComponent<NetWalker> ().StopCoroutine("MoveFromTo");
			}
            else
            {
				GetComponent<NetWalker> ().StopCoroutine("MoveFromTo");
                //GetComponent<NetWalker>().SetPoint(GetComponent<NetWalker>().net.GetNearestPoint(transform.position));
				GetComponent<NetWalker> ().StopCoroutine("MoveFromTo");
			}
		}
	}
}
