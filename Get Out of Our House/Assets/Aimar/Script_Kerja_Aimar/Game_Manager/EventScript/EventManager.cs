using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    [SerializeField] private Queue<EventAction> eventActions;
    private EventAction currentEventAction;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    void Start()
    {
        Queue<EventAction> moveActions = new Queue<EventAction>(DataEvents.instance._ListOfMoveAction);
        Queue<EventAction> dialogueActions = new Queue<EventAction>(DataEvents.instance._ListOfDialogueAction);
        eventActions = CombinesQueue(moveActions, dialogueActions);
        eventActions = new Queue<EventAction>(eventActions.OrderBy(ac => ac.timerEvent));
        currentEventAction = eventActions.Dequeue();
        TimeManager.instance.OneSecondIntervalEventAction += Instance_OneSecondIntervalEventAction;
    }

    private void Instance_OneSecondIntervalEventAction(int timerEvent)
    {
        if(currentEventAction == null) return;
        if(currentEventAction.timerEvent == timerEvent)
        {
            currentEventAction.CheckConditionsRequired?.Invoke();
            if (!currentEventAction.AllConditionMet) return;
            currentEventAction.InvokeAction();
            Debug.Log(currentEventAction);
            if (eventActions.Count == 0) return;
            currentEventAction = eventActions.Dequeue();
            Instance_OneSecondIntervalEventAction(timerEvent);
        }
    }
    private Queue<EventAction> CombinesQueue<EventAction>(Queue<EventAction> queue01, Queue<EventAction> queue02)
    {
        Queue<EventAction> combinedQueue = new Queue<EventAction>();

        while(queue01.Count > 0 || queue02.Count > 0)
        {
            if(queue01.TryDequeue(out EventAction data01))
            {
                combinedQueue.Enqueue(data01);
            }
            if(queue02.TryDequeue(out EventAction data02))
            {
                combinedQueue.Enqueue(data02);
            }
        }
        return combinedQueue;
    }
    public void SetCurrentActionConditions(bool input)
    {
        if (!currentEventAction.AllConditionMet) return;
        currentEventAction.AllConditionMet = input;
    }
}
