using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    private List<Queue<EventAction>> allEvents;
    private EventAction currentEventAction;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    void Start()
    {
        Queue<EventAction> childmoveActions = new Queue<EventAction>(DataEvents.instance._ListOfChildMoveAction);
        Queue<EventAction> dadmoveActions = new Queue<EventAction>(DataEvents.instance._ListOfDadMoveAction);
        Queue<EventAction> mommoveActions = new Queue<EventAction>(DataEvents.instance._ListOfMomMoveAction);
        Queue<EventAction> dialogueActions = new Queue<EventAction>(DataEvents.instance._ListOfDialogueAction);
        Queue<EventAction> callActions = new Queue<EventAction> (DataEvents.instance._ListOfCallAction);
        Queue<EventAction> behaviourActions = new Queue<EventAction>(DataEvents.instance._ListOfBehaviourAction);
        
        childmoveActions = new Queue<EventAction>(childmoveActions.OrderBy(ac => ac.timerEvent));
        dadmoveActions= new Queue<EventAction>(dadmoveActions.OrderBy(ac => ac.timerEvent));
        mommoveActions = new Queue<EventAction>(mommoveActions.OrderBy(ac => ac.timerEvent));
        dialogueActions= new Queue<EventAction>(dialogueActions.OrderBy(ac => ac.timerEvent));
        callActions = new Queue<EventAction>(callActions.OrderBy(ac => ac.timerEvent));
        behaviourActions= new Queue<EventAction>(behaviourActions.OrderBy(ac => ac.timerEvent));

        allEvents = new List<Queue<EventAction>>
        {
            childmoveActions,
            dadmoveActions,
            mommoveActions,
            dialogueActions,
            callActions,
            behaviourActions
        };

        TimeManager.instance.OneSecondIntervalEventAction += Instance_OneSecondIntervalEventAction;
    }

    private void Instance_OneSecondIntervalEventAction(int timerEvent)
    {
        foreach(Queue<EventAction> queue in allEvents)
        {
            if (queue.Count <= 0) continue;
            EventAction peekEvent = queue.Peek(); // Peek instead of Dequeue
            if (peekEvent.timerEvent == timerEvent)
            {
                currentEventAction = queue.Dequeue();
                currentEventAction.CheckConditionsRequired?.Invoke();
                if (currentEventAction.AllConditionMet)
                {
                    currentEventAction.InvokeAction();
                }
            }
        }
    }
    
    private Queue<EventAction> CombinesQueue(Queue<EventAction> queue01, Queue<EventAction> queue02)
    {
        Queue<EventAction> combinedQueue = new Queue<EventAction>();

        foreach (EventAction action in queue01)
        {
            combinedQueue.Enqueue(action);
        }

        foreach (EventAction action in queue02)
        {
            combinedQueue.Enqueue(action);
        }

        return combinedQueue;
    }
    public EventAction GetCurrentAction()
    {
        return currentEventAction;
    }
    public void SetCurrentActionConditions(bool input)
    {
        if (!currentEventAction.AllConditionMet) return;
        currentEventAction.AllConditionMet = input;
    }
}
