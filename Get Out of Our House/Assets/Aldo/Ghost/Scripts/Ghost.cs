using DialogueEditor;
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
    public static float speed = 9;
    private static int UltimateMoveAccumulation;
    private static int UltimateMoveLimit;
    private Rigidbody2D rb;
    private Animator animator;
    public Transform GhostPowerEffect;
    public static bool canMove = true;
    public static bool isPosessingObject;
    public static bool isPosessingPerson;
    public static bool isPosessing;
    public NPC npcPosessed;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Room currentRoom;
    [SerializeField] private LayerMask visible;
    [SerializeField] private LayerMask invisible;

    public event Action TakeDamage;
    public int health = 3;
    public DialogueScriptableObject dialogue;
    private PosessPerson possessPerson;
    private PossesingObject possessObject;
    private GameObject currentGameObject;
    private void Awake()
    {
        possessPerson = GetComponent<PosessPerson>();
        possessObject = GetComponent<PossesingObject>();
        IsUltimateForm = false;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        UltimateMoveAccumulation = 0;
        UltimateMoveLimit = 1;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //currentRoom.PlayParticleSystem(true);
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        PlayerControllerManager.instance.InvokeUltimate += Instance_InvokeUltimate;
        DialogueManager.instance.endDialogue += Begin_Conversation;
        DialogueManager.instance.beginDialogue += End_Conversation;
        //SoundManager.instance.normalAmbient.Play();
    }
    private void Begin_Conversation()
    {
        PlayerControllerManager.instance.InvokeInterract -= Instance_InvokeInterract;
        canMove = false;
    }
    private void End_Conversation()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        canMove = true;
    }
    private void Instance_InvokeUltimate()
    {
        if (UltimateMoveAccumulation < UltimateMoveLimit) return;
        StartCoroutine(UltimateActive());
    }
    private IEnumerator UltimateActive()
    {
        UltimateMoveAccumulation = 0;
        spriteRenderer.color = Color.black;
        IsUltimateForm = true;
        yield return new WaitForSeconds(3.3f);
        IsUltimateForm = false;
        spriteRenderer.color = Color.white;
        animator.SetBool("PowerUp", false);
        GhostPowerEffect.gameObject.SetActive(false);
    }
    public void UseUltimateEffect()
    {
        IsUltimateForm = false;
        spriteRenderer.color = Color.white;
        GhostPowerEffect.gameObject.SetActive(false);
    }
    private void Instance_InvokeInterract()
    {
        Debug.Log("Invoke Interract");
        if (currentGameObject == null) return;
        if (currentGameObject.tag == "PeopleInside")
        {
            Debug.Log("Invoke Possess Person");
            possessPerson.Instance_InvokeInterract(this, currentGameObject);
        }
        else if(currentGameObject.TryGetComponent<Environment_Door>(out Environment_Door door)){
            Debug.Log("Invoke Interract Door");
            door.InterractDoor(this);
        }
        else if (currentGameObject.tag == "object")
        {
            Debug.Log("Invoke Possess Object");
            possessObject.Instance_InvokeInterract(this, currentGameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        AssignNearestInterractableItems();
        if (UltimateMoveAccumulation >= UltimateMoveLimit)
        {
            animator.SetBool("PowerUp", true);
        }
        if (!canMove) 
        if (isPosessingObject || isPosessingPerson)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            canMove = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            canMove = true;
        }
        if (!canMove) return;
        moving();
    }
    public void AssignNearestInterractableItems()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
        if (colliders.Length > 0)
        {
            Collider2D nearestCollider = null;
            float nearestDistance = Mathf.Infinity;
            foreach (Collider2D collider in colliders)
            {
                //Debug.Log(collider.ToString() + (collider.CompareTag("Floor") || collider.CompareTag("People") || collider.CompareTag("Player")));
                if (collider.CompareTag("Floor") || collider.CompareTag("People") || collider.CompareTag("Player")) continue;
                if (IsUltimateForm && !collider.CompareTag("PeopleInside")) continue;
                if (collider.TryGetComponent<NPC>(out NPC npc))
                {
                    if (isPosessing) continue;
                    if (npc.fearMeter < 3) return;
                    else
                    {
                        nearestCollider = collider;
                        currentGameObject = nearestCollider.gameObject;
                        return;
                    }
                }
                Vector3 colliderPosition = collider.transform.position;
                if (collider.CompareTag("PeopleInside"))
                {
                    if (collider.TryGetComponent<GhostBuster>(out GhostBuster ghostBuster))
                    {
                        colliderPosition += ghostBuster.CenterPosition.position;
                    }
                }
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestCollider = collider;
                    nearestDistance = distance;
                }
            }
            if (nearestCollider != null)
            {
                //
                //Debug.Log(nearestCollider.gameObject.ToString());
                //if (currentGameObject == null || currentGameObject != nearestCollider.gameObject) return;
                if(nearestCollider.TryGetComponent<I_InterractableVisual>(out I_InterractableVisual interractableObject))
                {
                    if(currentGameObject != null)
                    {
                        if (currentGameObject.TryGetComponent<I_InterractableVisual>(out I_InterractableVisual oldInterractableObject))
                        {
                            oldInterractableObject.SetLightInterractableVisual(false);
                        }
                    }
                    interractableObject.SetLightInterractableVisual(true);
                    currentGameObject = nearestCollider.gameObject;
                }
            }
            else
            {
                if (currentGameObject != null)
                {
                    if (currentGameObject.TryGetComponent<I_InterractableVisual>(out I_InterractableVisual interractableVisual))
                    {
                        interractableVisual.SetLightInterractableVisual(false);
                    }
                    currentGameObject = null;
                }
            }
        }
        else
        {
            if(currentGameObject != null)
            {
                if (currentGameObject.TryGetComponent<I_InterractableVisual>(out I_InterractableVisual interractableVisual))
                {
                    interractableVisual.SetLightInterractableVisual(false);
                }
                currentGameObject = null;
            }
        }
    }

    void moving()
    {
        Vector2 inputMovement = PlayerControllerManager.instance.GetVector2Input();
        float theSpeed = IsUltimateForm ? speed * 2 : speed;
        currentRoom.GetGroundHorizontalBoundForPlayer(out float min, out float max);
        float xPosition = transform.position.x;
        if (xPosition >= min && xPosition <= max)
        {
            rb.velocity = new Vector2(inputMovement.x * theSpeed, inputMovement.y * theSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
            float clampedX = Mathf.Clamp(xPosition, min, max);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
        if (inputMovement.x < 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (inputMovement.x > 0)
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
        GhostPowerEffect.gameObject.SetActive(!input);
        if(UltimateMoveAccumulation < UltimateMoveLimit) GhostPowerEffect.gameObject.SetActive(false);
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
    public void Damage()
    {
        health--;
        TakeDamage?.Invoke();
        if(health <= 0)
        {
            EndingManager.instance.currentEndingType = EndingManager.EndingType.Lose;
            LevelManager.instance.EndGame(dialogue);
        }
    }
    public Animator getAnimator() => animator;
}
