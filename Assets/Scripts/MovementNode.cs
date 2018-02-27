using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Node.
/// </summary>
public class MovementNode : MonoBehaviour
{

	/// <summary>
	/// The connections (neighbors).
	/// </summary>
	[SerializeField]
	protected List<MovementNode> m_Connections = new List<MovementNode> ();

	/// <summary>
	/// Gets the connections (neighbors).
	/// </summary>
	/// <value>The connections.</value>
	public virtual List<MovementNode> connections
	{
		get
		{
			return m_Connections;
		}
	}

	public MovementNode this [ int index ]
	{
		get
		{
			return m_Connections [ index ];
		}
	}

	void OnValidate ()
	{
		
		// Removing duplicate elements
		m_Connections = m_Connections.Distinct ().ToList ();
	}
	
}
