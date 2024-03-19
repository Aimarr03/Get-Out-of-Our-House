using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuster : MonoBehaviour
{
    private GhostBuster_Move_Action moveAction;
    [SerializeField] private Room currentRoom;
    private Ghost ghost;
    private float currentUndetectedDurationGhost;
    [SerializeField] private float maxUndetectedDurationGhost;
    public event Action<bool, Room> GhostDetected;
    private void Awake()
    {
        moveAction = GetComponent<GhostBuster_Move_Action>();
    }
    private void Update()
    {
        
    }
    public GhostBuster_Move_Action GetMoveAction() => moveAction;
    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }
    public void GetBoundsRoom(out float min, out  float max)
    {
        currentRoom.GetGroundHorizontalBound(out min, out max);
    }
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
    public Ghost IsGhostDetected()
    {
        return ghost;
    }
    public void SetGhostDetected(Ghost ghost)
    {
        Room currentGhostRoom = null;
        if (this.ghost != null) currentGhostRoom = this.ghost.GetCurrentRoom();
        Debug.Log(currentGhostRoom);
        this.ghost = ghost;
        GhostDetected?.Invoke(this.ghost != null, currentGhostRoom);
    }

    public void RemoveGhostDetection(Ghost ghost)
    {
        Room currentGhostRoom = ghost.GetCurrentRoom();
        Debug.Log(currentGhostRoom);
        this.ghost = null;
        GhostDetected?.Invoke(this.ghost != null, currentGhostRoom);
    }
    public Environment_Door GetDoorTowardsCertainRoom(Room room)
    {
        Debug.Log(room);
        return currentRoom.GetDoorToRoom(room);
    }
}
