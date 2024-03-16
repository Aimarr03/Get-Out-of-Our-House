using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock_UI : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        TimeManager.instance.OneSecondIntervalEventUI += Instance_OneSecondIntervalEventUI;
    }

    private void Instance_OneSecondIntervalEventUI(TimeManager.TimeData timeData)
    {
        int hour = timeData.hour;
        int minute = timeData.minute;
        TimeManager.TwelveHourClockState timeState = timeData.timeState;

        //Below is to make sure the format is the same clockwise
        string hourString = hour < 10 ? "0" + hour : hour.ToString();
        string minuteString = minute < 10 ? "0" + minute : minute.ToString();
        string timeStateString = timeState == TimeManager.TwelveHourClockState.AM ? "AM" : "PM";
        setTime(hourString, minuteString, timeStateString);
    }

    public void setTime(string hour, string minute, string timeState)
    {
        string timeFormat = hour + ":" + minute + " " + timeState;
        textUI.text = timeFormat;
    }
}
