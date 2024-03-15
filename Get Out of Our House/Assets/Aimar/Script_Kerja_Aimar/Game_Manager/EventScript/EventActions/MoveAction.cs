using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MoveAction : EventAction
{
    [SerializeField] private List<Transform> DestinationLocation;

    public override bool CheckActionCondition()
    {
        return false;
    }

    public override void InvokeAction()
    {
        Debug.Log("Invoking Move Action");
        InvokeMoveAction();   
    }
    public async void InvokeMoveAction()
    {
        Queue<Transform> transformList = new Queue<Transform>(DestinationLocation);
        while(transformList.Count > 0)
        {
            Transform currentLocation = transformList.Dequeue();
            Debug.Log(currentLocation.gameObject.ToString());
            NPC_Move_Action npcMoveAction = npc.GetMoveAction();
            await npcMoveAction.SetTargetLocation(currentLocation.position);
        }
        Debug.Log("Done");
    }
}
