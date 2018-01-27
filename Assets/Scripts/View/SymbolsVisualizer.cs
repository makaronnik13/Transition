using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SymbolsVisualizer : MonoBehaviour
{
    //public Transmission testTransmission;

    private void Start()
    {
        TransmissionManager.Instance.OnTransmissionRecieved += DrawTransmission;
        TransmissionManager.Instance.OnTransmissionClosed += Hide;
    }

    //[Button]
    //private void TestDraw ()
    //{
    //    DrawTransmission(testTransmission);
    //}

    private void DrawTransmission (Transmission transmission)
    {
        foreach (Symbol symbol in transmission.content)
        {
            GameObject symbolGObj = new GameObject();
			symbolGObj.transform.SetParent(transform, false);
			symbolGObj.transform.localScale = Vector3.one;
			symbolGObj.AddComponent<Image>();
			symbolGObj.GetComponent<Image>().sprite = symbol.image;
        }
    }

    private void Hide (Transmission transmission, Choice choice)
    {
        foreach (Transform symbolChild in transform)
        {
            Destroy(symbolChild);
        }
    }
}
