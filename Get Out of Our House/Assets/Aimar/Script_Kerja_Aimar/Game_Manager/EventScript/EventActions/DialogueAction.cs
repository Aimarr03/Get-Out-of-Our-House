using System;
using System.Diagnostics;

[Serializable]
public class DialogueAction : EventAction
{
    public string dialogueName;
    public override bool CheckActionCondition()
    {
        return false;
    }

    public override void InvokeAction()
    {
        DialogueManager.instance.AssignDialogue(dialogueName);
    }

    
}
