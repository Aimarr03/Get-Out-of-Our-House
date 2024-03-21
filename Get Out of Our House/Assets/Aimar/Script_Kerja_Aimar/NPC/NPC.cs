using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Room currentRoom;
    [SerializeField] private Animator animator;
    [SerializeField] private DialogueAction dialogueAction;
    private NPC_Move_Action moveAction;

    public enum StateDirection
    {
        Left,
        Right
    }
    public StateDirection currentDirection = StateDirection.Left;
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
        Debug.Log(dialogueAction == null);
        if(dialogueAction != null)
        {
            dialogueAction.PreExecuteDialogueCondition?.Invoke();
            if (dialogueAction.AllConditionMet) DialogueManager.instance.AssignDialogue(dialogueAction.dialogueName);
            dialogueAction = null;
        }
    }
    public void FlippingSprite(Vector3 comparison)
    {
        Vector3 direction = transform.position - comparison;
        if (direction.x != 0)
        {
            float rotation_y = direction.x > 0 ? 0 : 180;
            currentDirection = rotation_y == 0 ? StateDirection.Left : StateDirection.Right;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation_y, 0));
        }
    }
    public void SetRoom(Room room) => currentRoom = room;
    public Room GetRoom() => currentRoom;
    public Animator GetAnimator() => animator;
    public void SubscribeToRoom(Room room) => room.playerEnterRoom += ExecuteDialogue;
    public void DesubscribeToRoom(Room room) => room.playerEnterRoom -= ExecuteDialogue;
    public NPC_Move_Action GetMoveAction() => moveAction;
    public void SetDialogueAction(DialogueAction dialogueAction) => this.dialogueAction = dialogueAction;
    public void TestingMakeFalse() => EventManager.Instance.SetCurrentActionConditions(false);

}
