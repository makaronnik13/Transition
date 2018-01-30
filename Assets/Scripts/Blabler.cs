using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Blabler : MonoBehaviour {

    private AudioClip[] DefaultClips, AngryClips, ScaryClips, QuestionClips, SadClips, HapyClips;
    private PersonObject person;
	
    public void Init()
    {
        TransmissionManager.Instance.OnTransmissionRecieved += RandomBla;
    }

    private void RandomBla(Transmission tr, Person p)
    {
        if (p!=person)
        {
            return;
        }

        AudioClip[] clips;

        switch (tr.emotion)
        {
            case PersonEmotionController.Emotion.Angry:
                clips = AngryClips;
                break;
            case PersonEmotionController.Emotion.Happy:
                clips = HapyClips;
                break;
            case PersonEmotionController.Emotion.Sad:
                clips = SadClips;
                break;
            case PersonEmotionController.Emotion.Scary:
                clips = ScaryClips;
                break;
            default:
                clips = DefaultClips;
                break;
        }

        GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
    }
}
