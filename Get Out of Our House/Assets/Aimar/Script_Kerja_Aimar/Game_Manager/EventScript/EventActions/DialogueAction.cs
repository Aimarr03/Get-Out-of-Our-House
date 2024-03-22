using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.Events;
using DialogueEditor;

[Serializable]
public class DialogueAction : EventAction
{
    public DialogueScriptableObject npcConversation;
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
