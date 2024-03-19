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
        SetTargetNextRoom();
    }
    private void Update()
    {
        Transform camera = Camera.main.transform;
        camera.position = new Vector3(transform.position.x, camera.position.y, camera.position.z);
    }
    private IEnumerator MoveActionCoroutine()
    {
        //Debug.Log(Vector3.Distance(targetLocation, transform.position));
        while (Vector3.Distance(targetLocation, transform.position) > 0.15f)
        {
            MovingTowards();
            yield return null;  
        }
        if(currentBounds <= 0)DetectionBox();
    }
    private IEnumerator SetTargetLocation(float min, float max)
    {
        float result = Random.Range(min, max);
        Vector3 newPosition = new Vector3(result, transform.position.y, 0);
        targetLocation = newPosition;
        yield return MoveActionCoroutine();
        yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
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
        //Debug.Log("Set Target Location");
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation;
        //StopCoroutine(MoveActionCoroutine());
        yield return MoveActionCoroutine();
        yield return new WaitForSeconds(Random.Range(0.8f, 1.3f));
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
        transform.position = Vector3.MoveTowards(currentPosition, targetLocation, movementSpeed * Time.deltaTime);
    }
    private void SetTargetNextRoom()
    {
        Room room = ghostBuster.GetCurrentRoom();
        Environment_Door nextRoom = room.GetRandomDoors();
        Vector3 nextRoomPosition = nextRoom.transform.position;
        Debug.Log($"Ghost Buster Wants to go to {nextRoom}");
        //StopCoroutine(SetTargetLocation(nextRoomPosition));
        StartCoroutine(SetTargetLocation(nextRoomPosition));
    }
    public void StartIdlingTheRoom()
    {
        Debug.Log("Ghost Buster Idling");
        Room room = ghostBuster.GetCurrentRoom();
        room.GetBackgroundHorizontalBound(out float minBound, out float maxBound);
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
    
}
