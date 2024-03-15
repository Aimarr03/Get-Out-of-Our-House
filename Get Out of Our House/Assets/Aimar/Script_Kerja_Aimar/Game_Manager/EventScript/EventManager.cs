using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Queue<EventAction> eventActions;
    private EventAction currentEventAction;
    // Start is called before the first frame update
    void Start()
    {
        /*Queue<EventAction> moveActions = new Queue<EventAction>(DataEvents.instance._ListOfMoveAction);
        Queue<EventAction> dialogueActions = new Queue<EventAction>(DataEvents.instance._ListOfDialogueAction);
        */
        eventActions = new Queue<EventAction>(DataEvents.instance._ListOfDialogueAction);
        currentEventAction = eventActions.Dequeue();
        TimeManager.instance.OneSecondIntervalEventAction += Instance_OneSecondIntervalEventAction;
    }

    private void Instance_OneSecondIntervalEventAction(int timerEvent)
    {
        if(currentEventAction == null) return;
        if(currentEventAction.timerEvent == timerEvent)
        {
            currentEventAction.InvokeAction();
            Debug.Log("Dialogue is executed");
        }
    }
    private Queue<T> CombinesQueue<T>(Queue<T> queue01, Queue<T> queue02)
    {
        Queue<T> combinedQueue = new Queue<T>();

        while(queue01.Count > 0 || queue02.Count > 0)
        {
            if(queue01.TryDequeue(out T data01))
            {
                combinedQueue.Enqueue(data01);
            }
            if(queue02.TryDequeue(out T data02))
            {
                combinedQueue.Enqueue(data02);
            }
        }
        return combinedQueue;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
