using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MoveAction : EventAction
{
    [SerializeField] private List<Environment_Door> DestinationLocation;

    public override int GetTimerEvent()
    {
        return timerEvent;
    }

    public override void InvokeAction()
    {
        Debug.Log("Invoking Move Action");
        InvokeMoveAction();   
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
