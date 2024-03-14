using System;
using UnityEngine;

[Serializable]
public abstract class EventAction : ScriptableObject
{
    public int timerEvent;
    public NPC npc;
    public abstract void InvokeAction();
}
