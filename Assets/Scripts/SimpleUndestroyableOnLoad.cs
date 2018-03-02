using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUndestroyableOnLoad : MonoBehaviour {
	void Awake() 
	{
		DontDestroyOnLoad(gameObject);
	}
}
