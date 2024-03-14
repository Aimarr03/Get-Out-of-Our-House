using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPC_Move_Action moveAction;
    private void Awake()
    {
        moveAction = GetComponent<NPC_Move_Action>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public NPC_Move_Action GetMoveAction()
    {
        return moveAction;
    }
}
