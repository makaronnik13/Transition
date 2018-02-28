
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalAniamtor : Singleton<JournalAniamtor> {

	public void Swith()
    {
        foreach (Person p in FindObjectsOfType<Person>())
        {
            //p.SwitchCollider();
        }
        GetComponent<Animator>().SetBool("Open", !GetComponent<Animator>().GetBool("Open"));
    }
}
