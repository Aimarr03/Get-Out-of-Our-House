using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Posessed : MonoBehaviour
{
    public NPCConversation npcConversation;
    public RuntimeAnimatorController controller;
    public bool isPosessed;
    private GameObject knive;
    public bool isHoldingKnive;
    [SerializeField] private Rigidbody2D rb;
    private float speed = 6;
    private NPC npc;
    // Start is called before the first frame update
    void Start()
    {
        npc = GetComponent<NPC>();
        rb = GetComponent<Rigidbody2D>();
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeAction;
        PlayerControllerManager.instance.InvokeAction1 += Instance_InvokeAction1;
    }

    private void Instance_InvokeAction1()
    {
        if (!isHoldingKnive) return;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(6, 8), 0);
        foreach(Collider2D collider in colliders)
        {
            Debug.Log(collider.gameObject);
            if(collider.TryGetComponent<NPC>(out NPC targetNPC))
            {
                if (targetNPC.type == NPC.NPC_Type.Child) continue;
                Debug.Log("Killed " + targetNPC.type);
                
            }
        }
    }

    void Instance_InvokeAction()
    {
        if (knive == null || !isPosessed) return;
        Debug.Log("Mengambil Knive");
        Destroy(knive);
        isHoldingKnive = true;
        npc.GetAnimator().runtimeAnimatorController = controller;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPosessed)
        {
            moving();
            CheckKnife();
        }
    }
    private void CheckKnife()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 4), 0);
        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.tag == "Knive")
            {
                knive = collider.gameObject;
            }
        }
    }
    void moving()
    {
        if (!isPosessed) return;
        if (npc == null || npc.GetAnimator() == null)
        {
            return;
        }
        Vector2 inputMovement = PlayerControllerManager.instance.GetVector2Input();
        if (inputMovement != Vector2.zero)
        {
            npc.GetAnimator().SetFloat("IsMoving", 1);
        }
        else
        {
            npc.GetAnimator().SetFloat("IsMoving", -1);
        }
        rb.velocity = new Vector2(inputMovement.x * speed, 0);
        if (inputMovement.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (inputMovement.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    
}
