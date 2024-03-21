using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAction : EventAction
{
    public Transform positionSpot;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
            
    }
}
