using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryVisualizer : MonoBehaviour {

	public Transform content;
	public GameObject dictionaryEntery;

	private List<Symbol> knownSymbols = new List<Symbol>();

	// Use this for initialization
	void Start () {
		TransmissionManager.Instance.OnTransmissionRecieved += AddSymbols;	
	}

	private void AddSymbols(Transmission transmission)
	{
		GameObject newEntery = Instantiate (dictionaryEntery);
		newEntery.transform.SetParent (content);
		newEntery.transform.localScale = Vector3.one;
		foreach(Symbol s in transmission.content)
		{
			if(!knownSymbols.Contains(s))
			{
				knownSymbols.Add (s);
				newEntery.GetComponent<DictionaryEntery> ().Init (s);
			}
		}
	}
}
