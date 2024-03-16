using System;
using System.Diagnostics;

[Serializable]
public class DialogueAction : EventAction
{
    public string dialogueName;
    public override bool CheckActionCondition(Action action)
    {
        return false;
    }

    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        DialogueManager.instance.AssignDialogue(dialogueName);
    }

    
}
