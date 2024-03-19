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
        targetPosition.y = nextDoor.room.GetFloorVerticalBound();

        npc.DesubscribeToRoom(room);
        npc.SubscribeToRoom(nextDoor.room);

        npc.transform.position = targetPosition;
        //Camera.main.transform.position = GetCameraNextDoorPosition();
    }
    public void InterractDoor(Ghost ghost)
    {
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = nextDoor.transform.position.y;
        ghost.transform.position = targetPosition;

        ghost.GetCurrentRoom().PlayParticleSystem(false);
        ghost.SetCurrentRoom(nextDoor.room);
        ghost.GetCurrentRoom().PlayParticleSystem(true);

        nextDoor.room.ExecutePlayerEnterRoomEvent();

        Camera.main.transform.position = GetCameraNextDoorPosition();
    }
    public void InterractDoor(GhostBuster ghostBuster)
    {
        Debug.Log("Ghost Buster interracting with door");
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = nextDoor.room.GetFloorVerticalBound();
        ghostBuster.transform.position = targetPosition;

        ghostBuster.SetCurrentRoom(nextDoor.room);
        ghostBuster.GetMoveAction().GetRandomizedMaxBounds();
        //Camera.main.transform.position = GetCameraNextDoorPosition();
        
        //ghostBuster.GetMoveAction().StartIdlingTheRoom();
    }
    private Transform GetCameraTransform()
    {
        return cameraPosition;
    }
    private Vector3 GetCameraNextDoorPosition()
    {
        return nextDoor.GetCameraTransform().position;
    }
    public Room GetRoom()
    {
        return room;
    }
}
