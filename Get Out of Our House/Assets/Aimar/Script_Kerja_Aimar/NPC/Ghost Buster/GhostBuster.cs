using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuster : MonoBehaviour
{
    private int sanity;
    private bool isVunerable;
    private int sanityArmor;
    private int maxSanityArmor;
    private GhostBuster_Move_Action moveAction;
    private Animator ghostBusterAnimator;
    [SerializeField] private Room currentRoom;
    private Ghost ghost;
    private float currentUndetectedDurationGhost;
    [SerializeField] private float maxUndetectedDurationGhost;
    public event Action<bool, Room> GhostDetected;
    private void Awake()
    {
        moveAction = GetComponent<GhostBuster_Move_Action>();
        ghostBusterAnimator = transform.GetChild(0).GetComponent<Animator>();
        isVunerable = false;
        maxSanityArmor = 2;
        sanityArmor = maxSanityArmor;
        sanity = 2;
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
    public bool Surprised()
    {
        sanityArmor--;
        if(sanityArmor <= 0)
        {
            isVunerable = true;
        }
        return sanityArmor <= 0;
    }
    public bool Fear()
    {
        if(isVunerable)
        {
            sanity--;
            sanityArmor = maxSanityArmor;
            isVunerable= false;
            if(sanity <= 0)
            {
                Debug.Log("Win");
            }
        }
        return sanity <= 0;
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
    public Animator GetAnimator()
    {
        return ghostBusterAnimator;
    }
    public void FlippingSprite(Vector3 comparison)
    {
        Vector3 direction = transform.position - comparison;
        if (direction.x != 0)
        {
            float rotation_y = direction.x > 0 ? 0 : 180;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation_y, 0));
        }
    }
}
