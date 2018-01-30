using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextVisualizer : MonoBehaviour
{
	private Image background;
	public Transform[] pivots;
    public Text text;

    private void Start()
    {
		background = transform.GetChild (0).GetComponent<Image> ();
        TransmissionManager.Instance.OnTransmissionRecieved += DrawTransmission;
		TransmissionManager.Instance.OnChoiceApplied += Hide;
    }

    private void DrawTransmission (Transmission transmission, Person p)
    {
        if (transmission.text != "")
        {
			text.enabled = true;
			text.text = transmission.text;
			
			//transform.SetParent(pivots[transmission.personId]);
			transform.localPosition = new Vector3 (850f, 0f, 0f);
			background.enabled = true;
        }
    }

    private void Hide (Choice choice)
    {
        text.enabled = false;
		background.enabled = false;
    }
}
