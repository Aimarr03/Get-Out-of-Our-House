using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GhostBuster_Move_Action : MonoBehaviour
{
    [SerializeField] private Vector3 targetLocation;
    [SerializeField] private Vector2 collisionSize;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxBounds;
    [SerializeField] private LayerMask interractableEnvironment;
    private int currentBounds;
    private GhostBuster ghostBuster;
    private void Awake()
    {
        ghostBuster = GetComponent<GhostBuster>();
    }
    private void Start()
    {
        ghostBuster.GhostDetected += GhostBuster_GhostDetected;
        SetTargetNextRoom();
    }
    private void GhostBuster_GhostDetected(bool detected, Room currentRecordRoomGhost)
    {
        if (detected)
        {
            Debug.Log("Ghost Detected");
            StopAllCoroutines();
        }
        else
        {
            Debug.Log("Ghost Gone");
            StopAllCoroutines();
            if (currentRecordRoomGhost != null) SetTargetNextRoom(currentRecordRoomGhost);
            else StartIdlingTheRoom();
        }
    }
    

    private void Update()
    {
        //Transform camera = Camera.main.transform;
        //camera.position = new Vector3(transform.position.x, camera.position.y, camera.position.z);
    }
    private IEnumerator MoveActionCoroutine()
    {
        //Debug.Log(Vector3.Distance(targetLocation, transform.position));
        //Debug.Log(Vector3.Distance(targetLocation, transform.position) > 0.15f);
        while (Vector3.Distance(targetLocation, transform.position) > 0.15f)
        {
            ghostBuster.GetAnimator().SetFloat("IsMoving", 1);
            MovingTowards();
            yield return null;  
        }
        if (currentBounds <= 0)
        {
            ghostBuster.GetAnimator().SetFloat("IsMoving", -1);
            DetectionBox();
        }
    }
    private IEnumerator SetTargetLocation(float min, float max)
    {
        float result = Random.Range(min, max);
        Vector3 newPosition = new Vector3(result, transform.position.y, 0);
        targetLocation = newPosition;
        yield return MoveActionCoroutine();
        ghostBuster.GetAnimator().StartPlayback();
        yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
        ghostBuster.GetAnimator().StopPlayback();
        currentBounds--;
        Debug.Log($"Current Bounds {currentBounds}");
        if (currentBounds > 0)
        {
            Debug.Log("Repeat Moving Idle");
            StartIdlingTheRoom();
        }
        else
        {
            Debug.Log("Done Moving Idle");
            SetTargetNextRoom();
        }
    }
    //This Set Target Location is for going to the door
    //Which after Invoking Move Action Coroutine, it will go idling which leads to randomize x axis of the floor min and max bounds
    //THis idling will iterate itself using Recursive until the limit reach 0 then go to the next room
    private IEnumerator SetTargetLocation(Vector3 targetLocation)
    {
        Debug.Log("Set Target Location " + targetLocation);
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation; 
        //StopCoroutine(MoveActionCoroutine());
        yield return MoveActionCoroutine();
        ghostBuster.GetAnimator().StartPlayback();
        yield return new WaitForSeconds(Random.Range(0.8f, 1.3f));
        ghostBuster.GetAnimator().StopPlayback();
        Debug.Log("Starting Idling");
        StartIdlingTheRoom();
    }
    
    public void GetRandomizedMaxBounds()
    {
        currentBounds = Random.Range(1, maxBounds + 1);
        Debug.Log($"Current Bounds {currentBounds}");
    }
    private void MovingTowards()
    {
        //Debug.Log("Moving Towards");
        Vector3 currentPosition = transform.position;
        ghostBuster.FlippingSprite(targetLocation);
        transform.position = Vector3.MoveTowards(currentPosition, targetLocation, movementSpeed * Time.deltaTime);
    }
    private void SetTargetNextRoom()
    {
        Room room = ghostBuster.GetCurrentRoom();
        Environment_Door nextRoom = room.GetRandomDoors();
        Vector3 nextRoomPosition = nextRoom.GetPosition();
        Debug.Log($"Ghost Buster Wants to go to {nextRoom}");
        //StopCoroutine(SetTargetLocation(nextRoomPosition));
        StartCoroutine(SetTargetLocation(nextRoomPosition));
    }
    private void SetTargetNextRoom(Room targetRoom)
    {
        Environment_Door doorTarget = ghostBuster.GetDoorTowardsCertainRoom(targetRoom);
        Vector3 nextRoomPosition = doorTarget.GetPosition();
        Debug.Log($"Ghost Buster Wants to go to {doorTarget}");
        StopAllCoroutines();
        currentBounds = 0;
        StartCoroutine(SetTargetLocation(nextRoomPosition));
    }
    public void StartIdlingTheRoom()
    {
        Debug.Log("Ghost Buster Idling");
        Room room = ghostBuster.GetCurrentRoom();
        room.GetGroundHorizontalBound(out float minBound, out float maxBound);
        //StopCoroutine(SetTargetLocation(minBound, maxBound));
        StartCoroutine(SetTargetLocation(minBound, maxBound));
    }
    private void DetectionBox()
    {
        if (currentBounds > 0) return;
        Collider2D[] collisionObject = Physics2D.OverlapBoxAll(transform.position, collisionSize, 0, interractableEnvironment);
        foreach (Collider2D currentObject in collisionObject)
        {
            Debug.Log(currentObject.gameObject.name);
            if (currentObject.gameObject.TryGetComponent<Environment_Door>(out Environment_Door environmentDoor))
            {
                environmentDoor.InterractDoor(ghostBuster);
            }
        }
    }
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }
}
