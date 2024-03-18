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
        currentBounds = 0;
    }
    private void Update()
    {
        if(currentBounds == 0)
        {

        }
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
    private void DetectionBox()
    {
        Collider2D[] collisionObject = Physics2D.OverlapBoxAll(transform.position, collisionSize, 0);
        foreach (Collider2D currentObject in collisionObject)
        {
            if (currentObject.gameObject.TryGetComponent<Environment_Door>(out Environment_Door environmentDoor))
            {
                environmentDoor.InterractDoor(ghostBuster);
            }
        }
    }
    public async Task SetTargetLocation(Vector3 targetLocation)
    {
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation;
        await MoveAction();
        await Task.Yield();
        currentBounds = Random.Range(1, maxBounds + 1);
    }

    public void MovingTowards()
    {
        //Debug.Log("Moving Towards");
        Vector3 currentPosition = transform.position;
        transform.position = Vector3.MoveTowards(currentPosition, targetLocation, movementSpeed * Time.deltaTime);
    }
}
