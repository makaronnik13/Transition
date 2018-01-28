using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFakeParalax : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Input.GetAxis("Mouse X")*0.1f*Vector3.right;
		transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(transform.position.x, -30, 30), transform.position.y, transform.position.z), Time.deltaTime);
	}
}
