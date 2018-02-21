using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerThoughtsVisualizer : MonoBehaviour {

    public TextMeshProUGUI text1;
    public Transform player;

    private Queue<string> phrases = new Queue<string>();

    private void Update()
    {
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(player.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
        GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
    }

    // Use this for initialization
    void Start()
    {
        text1.enabled = false;
        InvestigationManager.Instance.OnInvestigate += ShowFeedback;
    }

    private void ShowFeedback(string description)
    {
        List<string> ph = description.Split(new char[] {'|'}).ToList();


        foreach (string s in  ph)
        {
            phrases.Enqueue(s);
        }
        if (phrases.Count>0)
        {
            Show(phrases.Dequeue());
        }
    }


    private void Show(string s)
    {
        CancelInvoke("PushTheButton");
        text1.text = s;
        text1.enabled = true;
        Invoke("PushTheButton", s.Length/8);
    }

    private void PushTheButton()
    {
        if (phrases.Count > 0)
        {
            text1.text = phrases.Dequeue();
            Invoke("PushTheButton", text1.text.Length / 8);
        }
        else
        {     
            text1.enabled = false;     
        }
    }
}
