using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public static bool isPosessingObject;
    public static bool isPosessingPerson;
    public static bool isPosessing;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Room currentRoom;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentRoom.PlayParticleSystem(true);
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
    }

    private void Instance_InvokeInterract()
    {
        Debug.Log("Invoke Interract");
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, boxSize, 0);
        foreach (Collider2D collider2d in collider)
        {
            if (collider2d.TryGetComponent<Environment_Door>(out Environment_Door door))
            {
                door.InterractDoor(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moving();
        if (isPosessingObject || isPosessingPerson)
        {
            isPosessing = true;
        }
        else
        {
            isPosessing = false;
        }
        if(isPosessingPerson)
        {
            transform.position = GetComponent<PosessPerson>().targetPosess.transform.position;
        }
        
    }

    void moving()
    {
        Vector2 inputMovement = PlayerControllerManager.instance.GetVector2Input();
        
        //float movingX = Input.GetAxis("Horizontal");
        //float movingY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(inputMovement.x * speed , inputMovement.y * speed );
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 12;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 6;
        }
        if (inputMovement.x < 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        if (inputMovement.x > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
}
