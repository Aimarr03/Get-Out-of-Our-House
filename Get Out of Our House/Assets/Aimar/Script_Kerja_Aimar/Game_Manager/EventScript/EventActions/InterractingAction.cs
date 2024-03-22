using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InterractingAction : EventAction
{
    public float locationInterraction;
    public int duration;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        Debug.Log(npc + " Interracting");
        //npc.SetInterractionAction(this);
    }
}
