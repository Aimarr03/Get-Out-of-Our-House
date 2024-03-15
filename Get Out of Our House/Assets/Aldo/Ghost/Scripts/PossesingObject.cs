using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossesingObject : MonoBehaviour
{
    private GameObject otherObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(otherObject != null && otherObject.GetComponent<Objects>().canPosessed)
        {
            ReadyPossessObject();
        }
        else if(!Ghost.isPosessingObject && !Ghost.isPosessingPerson)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void ReadyPossessObject()
    {
        Debug.Log("Ready Posess Object");
        if (Input.GetKeyDown(KeyCode.F) && !Ghost.isPosessingObject)
        {
            Debug.Log("Posess Object");
            transform.position = otherObject.transform.position;
            otherObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            GetComponent<SpriteRenderer>().enabled = false;
            Ghost.isPosessingObject = true;
        }

        if (Ghost.isPosessingObject)
        {
            otherObject.GetComponent<Objects>().possess();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (otherObject == null && collision.tag == "object" && collision.GetComponent<Objects>().canPosessed)
        {
            otherObject = collision.gameObject;
            otherObject.GetComponent<SpriteRenderer>().color = Color.black;
            Debug.Log("Siap Merasuki " + gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (otherObject != null && collision.tag == "object")
        {
            otherObject.GetComponent<SpriteRenderer>().color = Color.white;
            otherObject = null;
            if (!Ghost.isPosessingPerson)
            {
                Ghost.isPosessingObject = false;
            }
        }
    }
}
