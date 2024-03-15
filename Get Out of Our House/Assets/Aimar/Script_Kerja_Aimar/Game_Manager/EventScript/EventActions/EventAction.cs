using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class EventAction
{
    public int timerEvent;
    public NPC npc;
    public abstract bool CheckActionCondition();
    public abstract void InvokeAction();
}
