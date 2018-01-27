using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamsVisualiser : MonoBehaviour {

	public GameParameter[] visibleParameters;
	public GameObject linePrefab;

	void Start()
	{
		foreach(GameParameter visualisedParam in visibleParameters)
		{
			GameObject newLine = Instantiate (linePrefab, transform);
			newLine.transform.localScale = Vector3.one;
			newLine.GetComponent<ParamPanel> ().Init (visualisedParam);
		}
	}
}
