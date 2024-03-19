using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public bool isPosessed = false;
    public bool canPosessed = true;
    public bool canShake = true;
    public bool canDrop = false;
    public bool canInteract = false;
    private GameObject targetPeople;
    private int a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControllerManager.instance.InvokeAction1 += Instance_InvokeAction1;
        PlayerControllerManager.instance.InvokeAction2 += Instance_InvokeAction2;
    }

    private void Instance_InvokeAction1()
    {
        if (!canShake || !isPosessed) return;
        canShake = false;
        canPosessed = false; 
        StartCoroutine(Shaking());
    }

    private void Instance_InvokeAction2()
    {
        if (!canDrop || !isPosessed) return;
        GetComponent<Rigidbody2D>().gravityScale = 10;
        GetComponent<Objects>().canPosessed = false;
    }

    /*public void possess()
    {
        
    }*/

    public IEnumerator Shaking()
    {
        // Yang dilakukan ketika Action 1 (animasi)
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        if (targetPeople != null)
        {
            targetPeople.GetComponent<FearMeter>().fearMeter += 5;
        }
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        canShake = true;
        canPosessed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "People")
        {
            targetPeople = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "People")
        {
            targetPeople = null;
        }
    }
}
