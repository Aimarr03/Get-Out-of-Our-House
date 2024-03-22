using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager instance;
    public enum EndingType
    {
        Ending_Fear,
        Ending_Murder,
        Ending_True
    }
    public EndingType currentEndingType;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    public EndingType GetEndingType(int index)
    {
        switch (index)
        {
            case 0: return EndingType.Ending_Fear;
            case 1: return EndingType.Ending_Murder;
            case 2: return EndingType.Ending_True;
            default:
                break;
        }
        return EndingType.Ending_Fear;
    }
    public void SetEndingType(int index)
    {
        currentEndingType = GetEndingType(index);
        Debug.Log("Change into " + currentEndingType);
    }
    public void CheckEnding(int index)
    {
        EndingType requiredEndingType = GetEndingType(index);
        EventManager.Instance.SetCurrentActionConditions(currentEndingType == requiredEndingType);
    }
}
