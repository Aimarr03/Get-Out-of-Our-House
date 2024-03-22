using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEvents : MonoBehaviour
{
    [SerializeField]public List<MoveAction> _ListOfChildMoveAction;
    [SerializeField] public List<MoveAction> _ListOfMomMoveAction;
    [SerializeField] public List<MoveAction> _ListOfDadMoveAction;
    [SerializeField] public List<DialogueAction> _ListOfDialogueAction;
    [SerializeField] public List<CallAction> _ListOfCallAction;
    [SerializeField] public List<BehaviourAction> _ListOfBehaviourAction;
    public static DataEvents instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
}
