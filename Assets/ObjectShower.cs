using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShower : MonoBehaviour {

    public GameObject ShowerPrefab;
    private bool showing = false;
    private List<GameObject> showingTips = new List<GameObject>();

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        foreach (GameObject go in showingTips)
        {
            Destroy(go);
        }
        showingTips.Clear();
        showing = false;
    }

    private void Show()
    {
        if (showing)
        {
            return;
        }

        foreach (PointAndClickObject io in FindObjectsOfType<PointAndClickObject>())
        {
            GameObject go = Instantiate(ShowerPrefab);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one;
            go.transform.position = io.transform.position;
            showingTips.Add(go);
        }
        showing = true;
    }
}
