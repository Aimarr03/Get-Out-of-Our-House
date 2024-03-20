using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    public LayerMask ghostBusterLayerMask;
    public bool IsUltimateForm;
    public static event Action<Ghost> PosessingSomething;
    private SpriteRenderer spriteRenderer;
    public static float speed = 6;
    private static int UltimateMoveAccumulation;
    private static int UltimateMoveLimit;
    private Rigidbody2D rb;
    public static bool canMove = true;
    public static bool isPosessingObject;
    public static bool isPosessingPerson;
    public static bool isPosessing;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Room currentRoom;
    [SerializeField] private LayerMask visible;
    [SerializeField] private LayerMask invisible;
    private void Awake()
    {
        IsUltimateForm = false;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        UltimateMoveAccumulation = 0;
        UltimateMoveLimit = 1;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //currentRoom.PlayParticleSystem(true);
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        PlayerControllerManager.instance.InvokeUltimate += Instance_InvokeUltimate;
        //DialogueManager.instance.beginDialogue += Instance_beginDialogue;
        //DialogueManager.instance.endDialogue += Instance_endDialogue;
    }

    private void Instance_InvokeUltimate()
    {
        if (UltimateMoveAccumulation < UltimateMoveLimit) return;
        StartCoroutine(UltimateActive());
    }
    private IEnumerator UltimateActive()
    {
        spriteRenderer.color = Color.red;
        IsUltimateForm = true;
        yield return new WaitForSeconds(3.3f);
        IsUltimateForm = false;
        spriteRenderer.color = Color.white;
    }
    public void UseUltimateEffect()
    {
        IsUltimateForm = false;
        spriteRenderer.color = Color.white;
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
        if (IsUltimateForm) return;
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
        if (UltimateMoveAccumulation >= UltimateMoveLimit)
        {
            if (IsUltimateForm) Debug.Log("ULTIMATEEEE");
            else Debug.Log("Ultimate Ready");
        }
        /*Debug.Log("Person " + isPosessingPerson);
        Debug.Log("Object " + isPosessingObject);
        Debug.Log("Can Moving " + canMove);*/
        if (!canMove) //Debug.Log("Pernah False");
        if (isPosessingObject || isPosessingPerson)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            canMove = false;
            //gameObject.layer = 8; 
            //Debug.Log("Masuk Posessing Cuy!");
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            canMove = true;
            //gameObject.layer = 6; 
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
        float theSpeed = IsUltimateForm ? speed * 2 : speed;
        rb.velocity = new Vector2(inputMovement.x * theSpeed, inputMovement.y * theSpeed);
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
    public void SetInvisibility(bool input)
    {
        isPosessing = input;
        isPosessingObject = input;
        GetComponent<SpriteRenderer>().enabled = !input;
        canMove = !input;
        Debug.Log($"visible layer {LayerMask.NameToLayer("Ghost")}");
        Debug.Log($"invisible layer {LayerMask.NameToLayer("Ghost Invisible")}");
        gameObject.layer = input ? LayerMask.NameToLayer("Ghost Invisible") : LayerMask.NameToLayer("Ghost");
        PosessingSomething?.Invoke(this);
    }
    public static void AccumulatePower()
    {
        UltimateMoveAccumulation = Math.Clamp(UltimateMoveAccumulation + 1, 0,UltimateMoveLimit);
    }
}
