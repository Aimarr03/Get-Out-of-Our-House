using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPC_Type
    {
        Child,
        Dad,
        Mom
    }
    public NPC_Type type;
    [SerializeField] private Room currentRoom;
    [SerializeField] private Animator animator;
    [SerializeField] private DialogueAction dialogueAction;
    private NPC_Move_Action moveAction;
    public int fearMeter;
    public bool IsBusy;
    public bool isPanic;
    public event Action panicAttack;
    public float currentTimerPanic;
    public enum StateDirection
    {
        Left,
        Right
    }
    public StateDirection currentDirection = StateDirection.Left;
    private void Awake()
    {
        IsBusy = false;
        isPanic = false;
        fearMeter = 0;
        moveAction = GetComponent<NPC_Move_Action>();
        
        TimeManager.instance.OneSecondIntervalEventAction += Instance_OneSecondIntervalEventAction;
    }
    private void Start()
    {
        if (currentRoom != null) currentRoom.AddCharacter(gameObject);
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
    public void TriggerFear(Objects.ObjectType objectType, Objects prop)
    {
        switch (objectType)
        {
            case Objects.ObjectType.Lightable:
                if (type != NPC_Type.Child) return;
                fearMeter++;
                panicAttack?.Invoke();
                Debug.Log("Trigger Fear " + transform.ToString());
                currentTimerPanic = 0;
                PanicCooldown();
                break;
            case Objects.ObjectType.Fallable:
                fearMeter += 2;
                panicAttack?.Invoke();
                Debug.Log("Trigger Fear " + transform.ToString());
                currentTimerPanic = 0;
                PanicCooldown();
                break;
        }
    }
    public async void PanicCooldown()
    {
        while(currentTimerPanic < 15)
        {
            currentTimerPanic += Time.deltaTime;
        }
        isPanic = false;
        await Task.Yield();
    }
    public void SetBusy(Transform target)
    {
        IsBusy = true;
    }
    public void IsFear()
    {
        EventManager.Instance.SetCurrentActionConditions(!isPanic);
    }
}
