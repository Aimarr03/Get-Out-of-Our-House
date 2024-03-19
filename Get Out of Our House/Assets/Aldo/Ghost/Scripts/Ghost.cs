using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    public static float speed = 6;
    private Rigidbody2D rb;
    public static bool canMove = true;
    public static bool isPosessingObject;
    public static bool isPosessingPerson;
    public static bool isPosessing;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Room currentRoom;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //currentRoom.PlayParticleSystem(true);
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        //DialogueManager.instance.beginDialogue += Instance_beginDialogue;
        //DialogueManager.instance.endDialogue += Instance_endDialogue;
    }

    private void Instance_endDialogue()
    {
        PlayerControllerManager.instance.InvokeInterract -= Instance_InvokeInterract;
        canMove = true;
    }

    private void Instance_beginDialogue()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        canMove= false;
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
        Debug.Log("Person " + isPosessingPerson);
        Debug.Log("Object " + isPosessingObject);
        Debug.Log("Can Moving " + canMove);
        if (!canMove) Debug.Log("Pernah False");
        if (isPosessingObject || isPosessingPerson)
        {
            isPosessing = true;
            canMove = false;
            GetComponent<SpriteRenderer>().enabled = false;
            gameObject.layer = 8; 
            Debug.Log("Masuk Posessing Cuy!");
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            isPosessing = false;
            gameObject.layer = 6; 
            canMove = true;
        }
        if (!canMove) return;
        moving();
    }

    void moving()
    {
        Vector2 inputMovement = PlayerControllerManager.instance.GetVector2Input();
        //Debug.Log(inputMovement);
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
