using UnityEngine;
using System;
using System.Diagnostics;

[Serializable]
public class DialogueAction : EventAction
{
    public string dialogueName;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        DialogueManager.instance.AssignDialogue(dialogueName);
    }

    
}
