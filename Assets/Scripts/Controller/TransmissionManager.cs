using System;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.Linq;

public class TransmissionManager : Singleton<TransmissionManager>
{
    public Action OnDialogFinished = () => { };
    public Action<DialogStateNode> OnNodeIn = (DialogStateNode node) => { };
    public Action<DialogStatePath> OnPathGo = (DialogStatePath path) => { };
    public Action<Person> OnPersonChanged = (Person person) => { };
    private DialogStateNode currentState;
    private Person talkablePerson;

    public void RunNode (DialogNode node)
    {
        RunState(node.dialogState.StartNode);
    }

    private void RunState(DialogStateNode node)
    {
        OnNodeIn.Invoke(node);
        currentState = node;
    }

    public void SetTalkablePerson(Person person)
    {
        talkablePerson = person;
        OnPersonChanged.Invoke(talkablePerson);
        RunNode(person.CurrentNode);
    }

    public void SelectDialogVariant(int i)
    {
        DialogStatePath path = (DialogStatePath)currentState.pathes[i];

        OnPathGo.Invoke(path);

        DialogStateNode aimNode = (DialogStateNode)path.End;

        if (aimNode.nodeType == DialogStateNode.StateNodeType.exit)
        {
            FinishDialog();
        }
        if (aimNode.nodeType == DialogStateNode.StateNodeType.narrativeExit)
        {
            talkablePerson.CurrentNode = (DialogNode)aimNode.exitPath.End;
            RunNode(talkablePerson.CurrentNode);
        }
    }

    private void FinishDialog()
    {
        OnDialogFinished.Invoke();
        talkablePerson = null;
        currentState = null;
    }
}
