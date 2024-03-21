using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorMenu : MonoBehaviour
{
    public void Open()
    {
        StartMenuAnimation.AtFrontDoor = true;
    }
}
