using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Queue<EventAction> eventActions;
    private EventAction currentEventAction;
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.instance.OneSecondIntervalEventAction += Instance_OneSecondIntervalEventAction;
    }

    private void Instance_OneSecondIntervalEventAction(int timerEvent)
    {
        if(currentEventAction.timerEvent == timerEvent)
        {
            currentEventAction.InvokeAction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
