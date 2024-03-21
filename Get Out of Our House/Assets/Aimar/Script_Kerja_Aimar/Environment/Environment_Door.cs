using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Door : MonoBehaviour, I_InterractableVisual
{
    [SerializeField] private Transform LightField;
    [SerializeField] private Environment_Door nextDoor;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Room room;
    private Vector3 Position;
    private void Awake()
    {
        LightField = transform.GetChild(0);
        LightField.gameObject.SetActive(false);
    }
    private void Start()
    {
        Position = transform.position;
    }
    public void InterractDoor(NPC npc)
    {
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = nextDoor.room.GetFloorVerticalBound();

        room.RemoveCharacter(npc.gameObject);
        npc.DesubscribeToRoom(room);
        npc.SubscribeToRoom(nextDoor.room);
        nextDoor.room.AddCharacter(npc.gameObject);
        npc.transform.position = targetPosition;
        //Camera.main.transform.position = GetCameraNextDoorPosition();
    }
    public void InterractDoor(Ghost ghost)
    {
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = nextDoor.transform.position.y;
        ghost.transform.position = targetPosition;
        
        room.PlayParticleSystem(false);
        
        ghost.SetCurrentRoom(nextDoor.room);
        room.RemoveCharacter(ghost.gameObject);
        
        nextDoor.room.AddCharacter(ghost.gameObject);
        nextDoor.room.PlayParticleSystem(true);

        nextDoor.room.ExecutePlayerEnterRoomEvent();

        Camera.main.transform.position = GetCameraNextDoorPosition();
    }
    public void InterractDoor(GhostBuster ghostBuster)
    {
        Debug.Log("Ghost Buster interracting with door");
        Vector3 targetPosition = nextDoor.transform.position;
        targetPosition.y = nextDoor.room.GetFloorVerticalBound();
        ghostBuster.transform.position = targetPosition;

        room.RemoveCharacter(ghostBuster.gameObject);
        ghostBuster.SetCurrentRoom(nextDoor.room);
        nextDoor.room.AddCharacter(ghostBuster.gameObject);
        
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
    public Room GetRoomNextDoor()
    {
        return nextDoor.room;
    }
    public Room GetRoom()
    {
        return room;
    }
    public Vector3 GetPosition()
    {
        return Position;
    }

    public void SetLightInterractableVisual(bool input)
    {
        LightField.gameObject.SetActive(input);
    }
}
