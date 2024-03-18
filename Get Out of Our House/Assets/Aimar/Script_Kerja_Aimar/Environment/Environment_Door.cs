using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Door : MonoBehaviour
{
    [SerializeField] private Environment_Door nextDoor;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Room room;
    
    public void InterractDoor(NPC npc)
    {
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = npc.transform.position.y;
        npc.transform.position = targetPosition;
        Camera.main.transform.position = GetCameraNextDoorPosition();
    }
    public void InterractDoor(Ghost ghost)
    {
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = ghost.transform.position.y;
        ghost.transform.position = targetPosition;
        Camera.main.transform.position = GetCameraNextDoorPosition();
    }
    public void InterractDoor(GhostBuster ghostBuster)
    {
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = ghostBuster.transform.position.y;
        ghostBuster.transform.position = targetPosition;
        ghostBuster.SetCurrentRoom(nextDoor.GetRoom());
    }
    private Transform GetCameraTransform()
    {
        return cameraPosition;
    }
    public Vector3 GetCameraNextDoorPosition()
    {
        return nextDoor.GetCameraTransform().position;
    }
    private Room GetRoomNextDoor()
    {
        return nextDoor.GetRoom();
    }
    public Room GetRoom()
    {
        return room;
    }
}
