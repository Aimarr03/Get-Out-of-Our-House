using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BehaviourAction : EventAction
{
    public float direction;
    public int duration;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        npc.SetBehaviour(this);
    }
}
