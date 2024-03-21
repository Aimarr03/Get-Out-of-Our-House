using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //One Second Interval doesn't mean literal one second real life
    //It just how much is one second in the game for the interval
    public static TimeManager instance;
    #region Time Characteristics
    private int hour;
    private int minute;
    private int timerEvent;
    private TwelveHourClockState timeState;
    private float currentTime;
    private float oneSecondInterval;
    private float TestingRealTime;
    //This struct helps to make data transmission easier and organized, more into organized
    public struct TimeData
    {
        public TimeData(int hour, int minute, TwelveHourClockState timeState)
        {
            this.hour = hour;
            this.minute = minute;
            this.timeState = timeState;
        }
        public int hour;
        public int minute;
        public TwelveHourClockState timeState;
    }
    public enum TwelveHourClockState
    {
        AM,
        PM
    }
    #endregion


    #region Time Event
    //Making sure that UI Event and Action Event are different to make ORGANIZED
    public event Action<int> OneSecondIntervalEventAction;
    public event Action<TimeData> OneSecondIntervalEventUI;
    #endregion
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
        hour = 7;
        minute = 0;
        timeState = TwelveHourClockState.PM;
        currentTime = 0;
        oneSecondInterval = 0.66f;
        timerEvent = 0;
        TestingRealTime = 0;
    }
    private void Start()
    {
        //Making sure the it is apply when opening the scene
        OneSecondIntervalEventUI?.Invoke(new TimeData(hour, minute, timeState));
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        TestingRealTime += Time.deltaTime;
        //This approach is to make sure that the time one second is recorded
        //In order to not discard the remaining Time.DeltaTime like for example 1.1234 which 1234
        //IT will be saved using the method which OneSecondInterval()
        if(currentTime > oneSecondInterval)
        {
            OneSecondInterval();
        }
    }
    private void OneSecondInterval()
    {
        //Since one second has passed, using Time.deltaTime wouldn't result in one who number, so I make sure it is substract by one
        currentTime -= oneSecondInterval;
        
        //This will check if minute and hour are reach it's limit which then change the time format
        //using ++ I don't need to seperate the addition with if statement
        if(++minute == 60)
        {
            minute = 0;
            if(++hour == 12)
            {
                hour = 0;
                timeState = TwelveHourClockState.AM;
            }
        }
        timerEvent++;
        OneSecondIntervalEventAction?.Invoke(timerEvent);
        OneSecondIntervalEventUI?.Invoke(new TimeData(hour,minute,timeState));
    }
    public int GetTimerEvent()
    {
        return timerEvent;
    }
}
