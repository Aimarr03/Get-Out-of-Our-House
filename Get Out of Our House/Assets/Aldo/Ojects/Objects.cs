using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public bool canPosessed = true;
    public bool canShake = true;
    public bool canDrop = false;
    public bool canInteract = false;
    private GameObject targetPeople;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void possess()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canShake)
        {
            StartCoroutine(Shaking());
            Destroy(targetPeople);
            GetComponent<Objects>().canPosessed = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && canDrop)
        {
            GetComponent<Rigidbody2D>().gravityScale = 10;
            GetComponent<Objects>().canPosessed = false;
        }
        if (canInteract)
        {
            Debug.Log("Interacted");
        }
    }

    public IEnumerator Shaking()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
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
