using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CallAction : EventAction
{
    public Transform positionSpot;
    public UnityEvent actionInvoked;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        actionInvoked?.Invoke();
    }
}
