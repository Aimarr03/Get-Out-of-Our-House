using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.Events;

[Serializable]
public class DialogueAction : EventAction
{
    public UnityEvent PreExecuteDialogueCondition;
    public string dialogueName;
    public int timer;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        npc.SetDialogueAction(this);
    }

    
}
