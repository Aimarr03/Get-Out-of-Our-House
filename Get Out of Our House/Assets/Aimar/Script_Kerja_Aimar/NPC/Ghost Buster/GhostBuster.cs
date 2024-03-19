using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuster : MonoBehaviour
{
    private GhostBuster_Move_Action moveAction;
    [SerializeField] private Room currentRoom;

    private void Awake()
    {
        moveAction = GetComponent<GhostBuster_Move_Action>();
    }
    public GhostBuster_Move_Action GetMoveAction() => moveAction;
    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }
    public void GetBoundsRoom(out float min, out  float max)
    {
        currentRoom.GetBackgroundHorizontalBound(out min, out max);
    }
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
}
