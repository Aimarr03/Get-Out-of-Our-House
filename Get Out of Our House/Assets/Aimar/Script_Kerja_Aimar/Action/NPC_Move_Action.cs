using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private Vector2 collisionSize;
    [SerializeField] private float movementSpeed;
    
    private NPC npc;
    private void Awake()
    {
        npc = GetComponent<NPC>();
    }
    private void Start()
    {
        SetTargetLocation(TestingTargetLocation.position);
    }
    public async void MoveAction()
    {
        Debug.Log(Vector3.Distance(targetLocation, transform.position));
        while(Vector3.Distance(targetLocation, transform.position) > 0.15f)
        {
            MovingTowards();
            await Task.Yield();
        }
        DetectionBox();
    }
    private void DetectionBox()
    {
        Collider2D[] collisionObject = Physics2D.OverlapBoxAll(transform.position, collisionSize, 0);
        foreach (Collider2D currentObject in collisionObject)
        {
            if (currentObject.gameObject.TryGetComponent<Environment_Door>(out Environment_Door environmentDoor))
            {
                environmentDoor.InterractDoor(npc);
            }
        }
    }
    public void SetTargetLocation(Vector3 targetLocation)
    {
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation;
        MoveAction();
    }
    
    public void MovingTowards()
    {
        //Debug.Log("Moving Towards");
        Vector3 currentPosition = transform.position;
        transform.position = Vector3.MoveTowards(currentPosition, targetLocation, movementSpeed * Time.deltaTime);
    }
     
    public void StartAction()
    {
        
    }
    public void ContinueAction()
    {

    }
    public void StopAction()
    {
        
    }
}
