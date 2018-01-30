using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/Person")]
public class PersonObject : InvestigatedItem
{
    public PersonDialogs Dialogs;
    public AudioClip[] DefaultClips, AngryClips, ScaryClips, QuestionClips, SadClips, HapyClips;
    public Sprite[] EmotionSprites;

    public Sprite GetEmotionSprite(PersonEmotionController.Emotion em)
    {
        return EmotionSprites[(int)em];
    }
}
