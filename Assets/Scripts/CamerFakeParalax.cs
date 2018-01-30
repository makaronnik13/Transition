using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFakeParalax : MonoBehaviour {

    public float scrollOffset = 30;
	
	// Update is called once per frame
	void Update () {
		transform.position += Input.GetAxis("Mouse X")*0.1f*Vector3.right;
		transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(transform.position.x, -30, 30), transform.position.y, transform.position.z), Time.deltaTime);


        if (Input.mousePosition.y > Screen.height / 5)
        {
            if (Input.mousePosition.x < scrollOffset)
            {
                transform.position -= 0.3f * Vector3.right;
            }
            if (Input.mousePosition.x > Screen.width - scrollOffset)
            {
                transform.position += 0.3f * Vector3.right;
            }
        }
	}
}
