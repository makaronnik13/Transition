using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryVisualizer : MonoBehaviour {

	public Transform content;
	public GameObject dictionaryEntery;

	private HashSet<Symbol> knownSymbols = new HashSet<Symbol>();

	// Use this for initialization
	void Start () {
		TransmissionManager.Instance.OnTransmissionRecieved += AddSymbols;	
	}

	private void AddSymbols(Transmission transmission, Person p)
	{
		foreach(Symbol s in transmission.content)
		{
			//Debug.Log ();
			if(!knownSymbols.Contains(s))
			{
				GameObject newEntery = Instantiate (dictionaryEntery);
				newEntery.transform.SetParent (content);
				newEntery.transform.localScale = Vector3.one;
				newEntery.GetComponent<DictionaryEntery> ().Init (s);
			}
			knownSymbols.Add (s);
		}
	}
}
