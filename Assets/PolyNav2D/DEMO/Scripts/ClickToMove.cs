using UnityEngine;
using System.Collections.Generic;

//example
[RequireComponent(typeof(PolyNavAgent))]
public class ClickToMove : MonoBehaviour{
	
	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}

	void Update() {

		if (Input.GetMouseButtonDown(0))
			agent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	//Message from Agent
	void OnDestinationReached(){

		//do something here...
	}

	//Message from Agent
	void OnDestinationInvalid(){

		//do something here...
	}
}
