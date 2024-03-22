using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GhostBuster;
using static NPC;

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
    private Queue<Environment_Door> transformList;
    private bool isMovingByEvent;
    [SerializeField] private Vector3 targetLocation;
    [SerializeField] private Vector2 collisionSize;
    [SerializeField] private float movementSpeed;
    private NPC npc;
    public MoveAction moveAction;
    public int duration;
    private void Awake()
    {
        isMovingByEvent = false;
        npc = GetComponent<NPC>();
    }
    private void Start()
    {
        StartIdlingTheRoom();
        npc.panicAttack += Npc_panicAttack;
    }

    public void SetInterract(float position)
    {
        Debug.Log("Preparing to Interract");
        isMovingByEvent = false;
        Vector3 targPosition = new Vector3(position, transform.position.y, 0);
        npc.GetAnimator().SetFloat("IsMoving", -1);
        StopAllCoroutines();
        StartCoroutine(InterractAction(targPosition));
    }
    private IEnumerator InterractAction(Vector3 targetPosition)
    {
        Debug.Log(npc + " going to " + targetPosition);
        yield return SetTargetLocation(targetPosition);
        Debug.Log(npc + " is Busy doing their activity");
        npc.GetAnimator().SetBool("IsBusy", true);
        if(npc.type == NPC.NPC_Type.Child)
        {
            npc.showerParticle.Play();
        }
    }
    private void Npc_panicAttack()
    {
        isMovingByEvent = true;
        StopAllCoroutines();
        npc.GetAnimator().SetFloat("IsMoving", -1);
        Room room = npc.GetRoom();
        Environment_Door door = room.GetRandomDoors();
        StartCoroutine(PanicAction(door.transform.position));
    }
    private IEnumerator PanicAction(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(1.3f);
        Debug.Log("Going Ham and want to leave the room");
        yield return StartCoroutine(SetTargetLocation(targetPosition));
        Debug.Log("Start Idling");
        StartIdlingTheRoom();
    }
    private void DetectionBox()
    {
        //Debug.Log(npc +" is Detecting with panic status " +npc.isPanic);
        if (!isMovingByEvent) return;
        Collider2D[] collisionObject = Physics2D.OverlapBoxAll(transform.position, collisionSize, 0);
        foreach (Collider2D currentObject in collisionObject)
        {
            if (currentObject.gameObject.TryGetComponent<Environment_Door>(out Environment_Door environmentDoor))
            {
                environmentDoor.InterractDoor(npc);
            }
        }
    }
    public IEnumerator Test()
    {
        Debug.Log("NGENTOT");
        yield return new WaitForSeconds(0.01f);
    }
    public IEnumerator SetTargetLocation(Vector3 targetLocation)
    {
        Debug.Log("Set Target " + targetLocation);
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation;
        yield return MoveActionCoroutine();
    }
    public IEnumerator SetTargetLocation(float min, float max)
    {
        float result = Random.Range(min, max);
        Vector3 newPosition = new Vector3(result, transform.position.y, 0);
        targetLocation = newPosition;
        yield return MoveActionCoroutine();
        npc.GetAnimator().StartPlayback();
        yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
        npc.GetAnimator().StopPlayback();
        StartIdlingTheRoom();
    }
    private IEnumerator MoveActionCoroutine()
    {
        Debug.Log(npc + " Moving");
        while (Vector3.Distance(targetLocation, transform.position) > 0.15f)
        {
            if (ConversationManager.Instance.IsConversationActive) continue;
            npc.GetAnimator().SetFloat("IsMoving", 1);
            MovingTowards();
            yield return null;
        }
        DetectionBox();
        npc.GetAnimator().SetFloat("IsMoving", -1);
    }
    
    public void MovingTowards()
    {
        //Debug.Log("Moving Towards");
        Vector3 currentPosition = transform.position;
        npc.FlippingSprite(targetLocation);
        float movementSpeedModifed = npc.isPanic ? movementSpeed * 2 : movementSpeed;
        transform.position = Vector3.MoveTowards(currentPosition, targetLocation, movementSpeedModifed * Time.deltaTime);
    }
    public void StartIdlingTheRoom()
    {
        if (npc.type == NPC_Type.Child && npc.isPosessed) return;
        StopAllCoroutines();
        //Debug.Log("NPC Idling");
        Room room = npc.GetRoom();
        room.GetGroundHorizontalBound(out float minBound, out float maxBound);
        //StopCoroutine(SetTargetLocation(minBound, maxBound));
        StartCoroutine(SetTargetLocation(minBound, maxBound));
    }
    public void SetMoveAction(MoveAction moveAction)
    {
        if (npc.type == NPC_Type.Child && npc.isPosessed) return;
        this.moveAction = moveAction;
        StopAllCoroutines();
        transformList = new Queue<Environment_Door>(moveAction.DestinationLocation);
        StartCoroutine(StartMoveAction());
    }
    private IEnumerator StartMoveAction()
    {
        if(transformList.Count > 0)
        {
            isMovingByEvent = true;
            Environment_Door newTarget = transformList.Dequeue();
            //Debug.Log("List " + transformList.Count);
            Debug.Log("NPC " + npc + " going to " + newTarget.transform.position);
            yield return SetTargetLocation(newTarget.transform.position);
            StartCoroutine(StartMoveAction());
        }
        else
        {
            isMovingByEvent = false;
            Debug.Log("Done");
            Done();
        }
    }
    private void Done()
    {
        moveAction = null;
        StartIdlingTheRoom();
    }
    public void SetFreeRoaming()
    {
        isMovingByEvent = true;
        Debug.Log(npc + " is Set Free Roaming");
        Npc_panicAttack();
    }
}
