using System;
using UnityEngine.Events;

[Serializable]
public abstract class EventAction
{
    public bool AllConditionMet = true;
    public UnityEvent CheckConditionsRequired;
    public int timerEvent;
    public NPC npc;
    
    public abstract void InvokeAction();
    public abstract int GetTimerEvent();
}
