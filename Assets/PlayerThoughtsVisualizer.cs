using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerThoughtsVisualizer : MonoBehaviour {

    public Text text1, text2;
    public Image background, buttonImage;
    private Button button;
    private Queue<string> phrases = new Queue<string>();

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PushTheButton);
        button.enabled = false;
        text1.enabled = false;
        text2.enabled = false;
        background.enabled = false;
        buttonImage.enabled = false;
        InvestigationManager.Instance.OnInvestigate += ShowFeedback;
    }

    private void ShowFeedback(string description)
    {
        List<string> ph = Regex.Split(description, @"(?<=[.!?])").ToList();
        ph.RemoveAt(ph.Count-1);

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
        text1.text = s;
        text2.text = s;
        button.enabled = true;
        text1.enabled = true;
        text2.enabled = true;
        background.enabled = true;
        buttonImage.enabled = true;
    }

    private void PushTheButton()
    {
        if (phrases.Count > 0)
        {
            text1.text = phrases.Dequeue();
            text2.text = text1.text;
        }
        else
        {
            button.enabled = false;
            text1.enabled = false;
            text2.enabled = false;
            background.enabled = false;
            buttonImage.enabled = false;
        }
    }
}
