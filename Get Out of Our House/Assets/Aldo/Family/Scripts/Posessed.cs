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

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
    }

    void Instance_InvokeInterract()
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
        if (isPosessed)
        { 
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    void moving()
    {
        if (!isPosessed) return;
        Vector2 inputMovement = PlayerControllerManager.instance.GetVector2Input();
        rb.velocity = new Vector2(inputMovement.x * speed, 0);
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
            transform.localScale = new Vector2(2, transform.localScale.y);
        }
        if (inputMovement.x > 0)
        {
            transform.localScale = new Vector2(-2, transform.localScale.y);
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
