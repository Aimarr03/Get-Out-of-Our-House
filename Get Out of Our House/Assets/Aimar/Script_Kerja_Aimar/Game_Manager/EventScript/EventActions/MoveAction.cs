using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MoveAction : EventAction
{
    [SerializeField] private List<Environment_Door> DestinationLocation;
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
    public void AltInvokeMoveAction()
    {
        if(DestinationLocation.Count > 0)
        {
            Transform targetLocation = DestinationQueue.Dequeue().transform;

        }
    }
    public async void InvokeMoveAction()
    {
        Queue<Environment_Door> transformList = new Queue<Environment_Door>(DestinationLocation);
        while(transformList.Count > 0)
        {
            Transform currentLocation = transformList.Dequeue().transform;
            Debug.Log(currentLocation.gameObject.ToString());
            NPC_Move_Action npcMoveAction = npc.GetMoveAction();
            await npcMoveAction.SetTargetLocation(currentLocation.position);
            await Task.Delay(UnityEngine.Random.Range(600, 1200));
        }
        Debug.Log("Done");
    }
}
