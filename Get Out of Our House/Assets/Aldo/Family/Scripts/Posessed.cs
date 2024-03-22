using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Posessed : MonoBehaviour
{
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
        PlayerControllerManager.instance.InvokeAction1 += Instance_InvokeAction;
    }

    void Instance_InvokeAction()
    {
        if (knive == null) return;
        Debug.Log("Mengambil Knive");
        Destroy(knive);
        isHoldingKnive = true;
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPosessed)
        {
            moving();
        }
    }

    void moving()
    {
        if (!isPosessed) return;
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Knive" && isPosessed)
        {
            knive = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Knive")
        {
            knive = null;
        }
    }
}
