using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
    [SerializeField] private Animator posessedAnimator;
    [SerializeField] private Room currentRoom;
    [SerializeField] private Animator animator;
    [SerializeField] private DialogueAction dialogueAction;
    [SerializeField] private BehaviourAction behaviourAction;
    [SerializeField] public ParticleSystem showerParticle;
    private NPC_Move_Action moveAction;
    public int fearMeter;
    public bool IsBusy;
    public bool isPanic;
    public bool isPosessed;
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
        SubscribeToRoom(currentRoom);
        TimeManager.instance.OneSecondIntervalEventAction += Instance_OneSecondIntervalEventAction;
    }
    private void Start()
    {
        if (currentRoom != null) currentRoom.AddCharacter(gameObject);
    }
    private void Instance_OneSecondIntervalEventAction(int currentimer)
    {
        if(hasDialogue) DialogueCheckCondition();
        if (IsBusy) CheckInterraction();
    }
    private void CheckInterraction()
    {
        behaviourAction.duration--;
        if(behaviourAction.duration <= 0)
        {
            ClearInterraction();
        }
    }
    private void ClearInterraction()
    {
        Debug.Log(gameObject + "interraction finish");
        behaviourAction = null;
        IsBusy = false;
        animator.SetBool("IsBusy", false);
        moveAction.StartIdlingTheRoom();
        if(type == NPC_Type.Child)
        {
            SetVunerable();
            showerParticle.Stop();
        }
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
        if(hasDialogue)
        {
            ConversationManager.Instance.StartConversation(dialogueAction.npcConversation);
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
        if (type == NPC_Type.Child && isPosessed) return;
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
    public void SetDialogueAction(DialogueAction dialogueAction)
    {
        this.dialogueAction = dialogueAction;
        hasDialogue = true;
    }
    public void TestingMakeFalse() => EventManager.Instance.SetCurrentActionConditions(false);
    public int GetFear() => fearMeter;
    
    public void TriggerFear(Objects.ObjectType objectType, Objects prop)
    {
        if (!isVunerable) return;
        switch (objectType)
        {
            case Objects.ObjectType.Lightable:
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
        while(currentTimerPanic < 5)
        {
            currentTimerPanic += Time.deltaTime;
        }
        isPanic = false;
        moveAction.SetFreeRoaming();
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
        Debug.Log("Fear exceeded? " + (fearMeter >= parameter));
        EventManager.Instance.SetCurrentActionConditions(fearMeter >= parameter);
    }
    public void isPossess()
    {
        EventManager.Instance.SetCurrentActionConditions(!isPosessed);
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
