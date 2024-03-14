using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Move_Action : MonoBehaviour
{
    //This the first behaviour for the NPC
    //The target is the make the NPC to be able to move to one location to another dynamically
    //Let say that the NPC is in the bedroom and want to go to the bathroom, it need to go to the door from the current location
    //Interract to the door, which lead to teleport to the door of the other room
    //Check if the location is the same as the target room
    //If it not, it will repeat the cycle
    // It's yes, it will go to the target location of that room
    #region testing
    [SerializeField] private Transform TestingTargetLocation;
    #endregion
    [SerializeField] private Vector3 targetLocation;
    [SerializeField] private float movementSpeed;
    
    private bool canMove;
    private bool canInterractWithObject;
    private NPC npc;
    private void Awake()
    {
        npc = GetComponent<NPC>();
    }
    private void Start()
    {
        SetTargetLocation(TestingTargetLocation.position);
        StartAction();
    }
    private void Update()
    {
        if (!canMove) return;
        if (Vector3.Distance(targetLocation, transform.position) < 0.1f)
        {
            canMove = false;
        }
        MovingTowards();
    }
    public void SetTargetLocation(Vector3 targetLocation)
    {
        canMove = true;
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation;
    }
    
    public void MovingTowards()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = transform.position.y;
        transform.position = Vector3.MoveTowards(currentPosition, targetLocation, movementSpeed * Time.deltaTime);
    }
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canInterractWithObject) return;
        if (collision.TryGetComponent<Environment_Door>(out Environment_Door door))
        {
            door.InterractDoor(npc);
        }
    }
    public void StartAction()
    {
        canMove = true;
        canInterractWithObject = true;
    }
    public void ContinueAction()
    {

    }
    public void StopAction()
    {
        canMove = false;
        canInterractWithObject = false;
    }
}
