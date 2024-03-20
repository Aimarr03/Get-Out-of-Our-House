using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GhostBuster : MonoBehaviour
{
    public enum StateDirection
    {
        Left,
        Right
    }
    public StateDirection currentDirection = StateDirection.Left;
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
    private float maxTimerGhostGone;
    private void Awake()
    {

        moveAction = GetComponent<GhostBuster_Move_Action>();
        ghostBusterAnimator = transform.GetChild(0).GetComponent<Animator>();
        isVunerable = false;
        maxSanityArmor = 1;
        maxTimerGhostGone = 1.5f;
        sanityArmor = maxSanityArmor;
        sanity = 1;
    }
    private void Start()
    {
        Ghost.PosessingSomething += Ghost_PosessingSomething;
    }

    private void Ghost_PosessingSomething(Ghost input)
    {
        StopCoroutine(GhostDetectionLogicTimer(input));
        StartCoroutine(GhostDetectionLogicTimer(input));
    }
    private IEnumerator GhostDetectionLogicTimer(Ghost input)
    {
        if (Ghost.isPosessing && ghost != null && input.GetCurrentRoom() == GetCurrentRoom())
        {
            ghost = null;
            yield return new WaitForSeconds(maxTimerGhostGone);
        }
        else
        {
            if (input.GetCurrentRoom() == GetCurrentRoom())
            {
                ghost = input;
            }
        }
        GhostDetected?.Invoke(ghost != null, null);
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
        Debug.Log("Ghost Buster is Surprised");
        sanityArmor--;
        if(sanityArmor <= 0)
        {
            isVunerable = true;
        }
        return sanityArmor <= 0;
    }
    public bool Fear()
    {
        Debug.Log("Ghost is disturbed");
        if(isVunerable)
        {
            sanity--;
            sanityArmor = maxSanityArmor;
            isVunerable= false;
            if(sanity <= 0)
            {
                ghostBusterAnimator.SetBool("IsDead", true);
            }
        }
        return sanity <= 0;
    }
    public bool CheckVunerability()
    {
        return isVunerable;
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
            currentDirection = rotation_y == 0 ? StateDirection.Left : StateDirection.Right;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation_y, 0));
        }
    }
}
