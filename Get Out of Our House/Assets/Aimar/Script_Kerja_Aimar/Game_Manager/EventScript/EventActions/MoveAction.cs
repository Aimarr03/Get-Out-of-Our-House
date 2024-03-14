using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MoveAction", menuName = "Eveny Action/Create Move Action", order = 1)]
public class MoveAction : EventAction
{
    [SerializeField] private List<Transform> DestinationLocation;
    public override void InvokeAction()
    {
           
    }
}
