using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Door : MonoBehaviour
{
    [SerializeField] private Environment_Door nextDoor;
    [SerializeField] private Transform cameraPosition;
    
    public void InterractDoor(NPC npc)
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = 0;
        npc.transform.position = targetPosition;
        //Camera.main.transform.position = GetCameraNextDoorPosition();
    }
    private Transform GetCameraTransform()
    {
        return cameraPosition;
    }
    public Vector3 GetCameraNextDoorPosition()
    {
        return nextDoor.GetCameraTransform().position;
    }
}
