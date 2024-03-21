using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MoveAction : EventAction
{
    [SerializeField] public List<Environment_Door> DestinationLocation;
    private Queue<Environment_Door> DestinationQueue;
    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        Debug.Log("Invoking Move Action");
        DestinationQueue = new Queue<Environment_Door>(DestinationLocation);
        //AltInvokeMoveAction();
        InvokeMoveAction();   
    }
    public void InvokeMoveAction()
    {
        npc.GetMoveAction().SetMoveAction(this);
        Debug.Log("NPC " + npc + " Get " + this);
    }
}
