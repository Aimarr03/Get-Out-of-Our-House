using DialogueEditor;
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
    public bool isVunerable;
    [SerializeField] private Room currentRoom;
    [SerializeField] private Animator animator;
    [SerializeField] private DialogueAction dialogueAction;
    [SerializeField] private BehaviourAction behaviourAction;
    [SerializeField] public ParticleSystem showerParticle;
    private NPC_Move_Action moveAction;
    public int fearMeter;
    public bool IsBusy;
    public bool isPanic;
    public event Action panicAttack;
    public float currentTimerPanic;
    public bool hasDialogue;
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
        if (IsBusy) CheckInterraction();
    }
    private void CheckInterraction()
    {
        /*interractionAction.duration--;
        if(interractionAction.duration <= 0)
        {
            ClearInterraction();
        }*/
    }
    private void ClearInterraction()
    {
        /*Debug.Log(gameObject + "interraction finish");
        interractionAction = null;
        IsBusy = false;
        animator.SetBool("IsBusy", false);
        moveAction.StartIdlingTheRoom();
        if(type == NPC_Type.Child)
        {
            showerParticle.Stop();
        }*/
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
    public void SetBehaviour(BehaviourAction action)
    {
        Debug.Log("Behaviour Action Invoked");
        behaviourAction = action;
        IsBusy = true;
        moveAction.SetInterract(action.direction);
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
        if (!isVunerable) return;
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
                ClearInterraction();
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
    public void HasFear()
    {
        EventManager.Instance.SetCurrentActionConditions(fearMeter > 0);
    }
    public void CheckNotFear()
    {
        EventManager.Instance.SetCurrentActionConditions(fearMeter == 0);
    }
    public void CheckFearExceed(int parameter)
    {
        EventManager.Instance.SetCurrentActionConditions(fearMeter >= parameter);
    }
    public void SetVunerable()
    {
        Debug.Log(gameObject + " is now vunerable");
        isVunerable = true;
    }
    public void GetBUsyStatus()
    {
        Debug.Log(gameObject + " is Busy status " + IsBusy);
        EventManager.Instance.SetCurrentActionConditions(!IsBusy);
    }
}
