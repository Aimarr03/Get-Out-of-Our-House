using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour, I_InterractableVisual
{
    public enum ObjectType
    {
        Lightable,
        Fallable,
        Openable,
    }
    [SerializeField] Sprite spriteOpen;
    [SerializeField] private string dialogueName;
    public Room room;
    public bool isPosessed = false;
    public bool canPosessed = true;
    public bool canShake = true;
    public bool canDrop = false;
    public bool isOpen = false;
    private GameObject targetPeople;
    private Ghost ghost;
    private SpriteRenderer interractedVisual;
    [SerializeField] GameObject LightField;

    [SerializeField] private float radiusCollision;
    [SerializeField] private Sprite TouchFloorSprite;
    [SerializeField] private ObjectType objectType;
    [SerializeField] private Transform lightSource;
    [SerializeField] private bool currentInterract = true;
    // Start is called before the first frame update
    private void Awake()
    {
        LightField = transform.GetChild(0).gameObject;
        LightField.SetActive(false);
    }
    void Start()
    {
        interractedVisual = GetComponent<SpriteRenderer>();
        InterractEffect(currentInterract);
        
    }
    public void SetPossessObject()
    {
        PlayerControllerManager.instance.InvokeAction1 += Instance_InvokeAction1;
        PlayerControllerManager.instance.InvokeAction2 += Instance_InvokeAction2;
    }
    public void UnsetPosessedObject()
    {
        PlayerControllerManager.instance.InvokeAction1 -= Instance_InvokeAction1;
        PlayerControllerManager.instance.InvokeAction2 -= Instance_InvokeAction2;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void Instance_InvokeAction1()
    {
        if (!canShake || !isPosessed) return;
        switch (objectType)
        {
            case ObjectType.Fallable:
                break;
            case ObjectType.Lightable:
                canShake = false;
                canPosessed = false;
                StartCoroutine(Shaking());
                AttemptFearGhostBuster();
                break;
            case ObjectType.Openable:
                if (isOpen)
                {
                    DialogueManager.instance.AssignDialogue(dialogueName);
                    isOpen = false;
                    canPosessed = false;
                }
                
                break;
        }
        
    }
    public void SetOpen()
    {
        Debug.Log(gameObject + " is now open");
        isOpen = true;
        interractedVisual.enabled = false;
    }
    private void AttemptFearGhostBuster()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(8, 8), 0);
        Collider2D colliderPerson = null;
        foreach (Collider2D collision in collisions)
        {
            Debug.Log("Ghost Buster detected = "+ collision);
            if (collision.TryGetComponent<GhostBuster>(out GhostBuster ghostBuster))
            {
                colliderPerson = collision;
                Transform ghostPosition = colliderPerson.transform;
                Vector3 direction = (ghostPosition.position - transform.position);
                float x_direction = direction.x;
                if (x_direction > 0 && ghostBuster.currentDirection == GhostBuster.StateDirection.Right)
                {
                    ghostBuster.Surprised();
                    Ghost.AccumulatePower();
                }
                else if (x_direction < 0 && ghostBuster.currentDirection == GhostBuster.StateDirection.Left)
                {
                    ghostBuster.Surprised();
                    Ghost.AccumulatePower();
                }
                continue;
            }
            if(collision.TryGetComponent<NPC>(out NPC npc))
            {
                npc.TriggerFear(ObjectType.Lightable, this);
            }
        }
        if(colliderPerson != null)
        {
            
        }
    }
    private void Instance_InvokeAction2()
    {
        if (!canDrop || !isPosessed) return;
        switch (objectType)
        {
            case ObjectType.Fallable:
                if (!canDrop) break;
                canDrop = false;
                GetComponent<Rigidbody2D>().gravityScale = 10;
                GetComponent<Objects>().canPosessed = false;
                StartCoroutine(Fading());
                break;
            case ObjectType.Lightable:
                break;
        }
        Ghost.isPosessing = false;
    }

    /*public void possess()
    {
        
    }*/
    public void SetGhost(Ghost ghost) => this.ghost = ghost;
    public IEnumerator Fading()
    {
        yield return new WaitForSeconds(3f);
        if(ghost != null) ghost.SetInvisibility(false);
        Ghost.isPosessing = false;
        isPosessed = false;
        Destroy(gameObject);
    }

    public IEnumerator Shaking()
    {
        // Yang dilakukan ketika Action 1 (animasi)
        currentInterract = !currentInterract;
        InterractEffect(currentInterract);
        //GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        /*if (targetPeople != null)
        {
            targetPeople.GetComponent<FearMeter>().fearMeter += 5;
        }*/
        //GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //GetComponent<SpriteRenderer>().color = Color.white;
        canShake = true;
        canPosessed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "People")
        {
            targetPeople = collision.gameObject;
        }
        if(collision.tag == "Floor")
        {
            Debug.Log("Menyentuh Floor");
            if (room == null) return;
            interractedVisual.sprite = TouchFloorSprite;
            List<GameObject> ListOfPeople = room.GetPeople();
            foreach(GameObject person in ListOfPeople)
            {
                Debug.Log(person);
                if (person.TryGetComponent<GhostBuster>(out GhostBuster ghostBuster))
                {
                    if (ghostBuster.CheckVunerability())
                    {
                        Debug.Log("Trigger Surprise Fallable on " + ghostBuster);
                        ghostBuster.Fear();
                    }
                    else
                    {
                        Debug.Log("Trigger Surprise Fallable on " + ghostBuster);
                        ghostBuster.Surprised();
                    }
                }
                else if (person.TryGetComponent<NPC>(out NPC npc))
                {
                    Debug.Log("Trigger Fear Fallable on " + npc);
                    npc.TriggerFear(ObjectType.Fallable, this);
                }
            }
            ghost.SetInvisibility(false);
            Ghost.isPosessing = false;
            isPosessed = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "People")
        {
            targetPeople = null;
        }
    }
    private void InterractEffect(bool input)
    {
        interractedVisual.enabled = input;
        if (lightSource != null)
        {
            lightSource.gameObject.SetActive(input);
        }   
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusCollision);
    }

    public void SetLightInterractableVisual(bool input)
    {
        if(isPosessed) LightField.gameObject.SetActive(false);
        LightField.gameObject.SetActive(input);
    }
}
