using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEvents : MonoBehaviour
{
    [SerializeField]public List<MoveAction> _ListOfMoveAction;
    [SerializeField] public List<DialogueAction> _ListOfDialogueAction;
    public static DataEvents instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
}
