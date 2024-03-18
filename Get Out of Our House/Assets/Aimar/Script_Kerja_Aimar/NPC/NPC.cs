using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private DialogueAction dialogueAction;
    private NPC_Move_Action moveAction;
    private void Awake()
    {
        moveAction = GetComponent<NPC_Move_Action>();
        TimeManager.instance.OneSecondIntervalEventAction += Instance_OneSecondIntervalEventAction;
    }

    private void Instance_OneSecondIntervalEventAction(int currentimer)
    {
        if(dialogueAction != null) DialogueCheckCondition();
    }
    private void DialogueCheckCondition()
    {
        dialogueAction.timer--;
        if(dialogueAction.timer <= 0 )
        {
            dialogueAction = null;
        }
    }
    private void ExecuteDialogue()
    {
        if(dialogueAction != null)
        {
            dialogueAction.PreExecuteDialogueCondition.Invoke();
            if (dialogueAction.AllConditionMet) DialogueManager.instance.AssignDialogue(dialogueAction.dialogueName);
            dialogueAction = null;
        }
    }
    public void SubscribeToRoom(Room room)
    {
        room.playerEnterRoom += ExecuteDialogue;
    }
    public void DesubscribeToRoom(Room room)
    {
        room.playerEnterRoom -= ExecuteDialogue;
    }

    public NPC_Move_Action GetMoveAction()
    {
        return moveAction;
    }
    public void SetDialogueAction(DialogueAction dialogueAction)
    {
        this.dialogueAction = dialogueAction;
    }
    public void TestingMakeFalse()
    {
        EventManager.Instance.SetCurrentActionConditions(false);
    }
    
}
