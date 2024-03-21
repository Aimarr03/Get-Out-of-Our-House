using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterractingAction : EventAction
{
    public Transform locationInterraction;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        
    }
}
