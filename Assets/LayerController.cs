using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayerController : MonoBehaviour {

    public bool update = false;
    private SpriteRenderer SRenderer;

    private void Awake()
    {
        SRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start ()
    {
        SetLayer();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (update)
        {
            SetLayer();
        }
	}

    void SetLayer()
    {
        SRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y*10);
    }
}
