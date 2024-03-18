using System.Threading.Tasks;
using UnityEngine;

public class GhostBuster_Move_Action : MonoBehaviour
{
    [SerializeField] private Vector3 targetLocation;
    [SerializeField] private Vector2 collisionSize;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxBounds;


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
        
    }
    private async Task MoveAction()
    {
        //Debug.Log(Vector3.Distance(targetLocation, transform.position));
        while (Vector3.Distance(targetLocation, transform.position) > 0.15f)
        {
            MovingTowards();
            await Task.Yield();
        }
        DetectionBox();
    }
    private void SetTargetNextRoom()
    {
        Room room = ghostBuster.GetCurrentRoom();
        Debug.Log($"Going to {room.gameObject} room");
        Environment_Door nextRoom = room.GetRandomDoors();
        Vector3 nextRoomPosition = nextRoom.transform.position;
        SetTargetLocation(nextRoomPosition);
    }
    public async void StartIdlingTheRoom()
    {
        Room room = ghostBuster.GetCurrentRoom();
        room.GetBackgroundHorizontalBound(out float minBound, out float maxBound);
        SetTargetLocation(minBound, maxBound);
        await Task.Yield();
    }
    private void DetectionBox()
    {
        if (currentBounds != 0) return;
        Collider2D[] collisionObject = Physics2D.OverlapBoxAll(transform.position, collisionSize, 0);
        foreach (Collider2D currentObject in collisionObject)
        {
            Debug.Log(currentObject.gameObject.name);
            if (currentObject.gameObject.TryGetComponent<Environment_Door>(out Environment_Door environmentDoor))
            {
                environmentDoor.InterractDoor(ghostBuster);
            }
        }
    }
    private async Task SetTargetLocation(Vector3 targetLocation)
    {
        //Debug.Log("Going to the Door");
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation;
        await MoveAction();
    }
    private async void SetTargetLocation(float min, float max)
    {
        float result = Random.Range(min, max);
        Vector3 newPosition = new Vector3(result, transform.position.y, 0);
        targetLocation = newPosition;
        await MoveAction();
        await Task.Delay(Random.Range(800, 1500));
        currentBounds--;
        Debug.Log($"Current Bounds {currentBounds}");
        if (currentBounds > 0)
        {
            StartIdlingTheRoom();
        }
        else
        {
            SetTargetNextRoom();
        }
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
}
