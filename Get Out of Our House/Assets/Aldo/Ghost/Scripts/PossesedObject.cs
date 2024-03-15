using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossesedObject : MonoBehaviour
{
    private GameObject otherObject;
    private bool isPosessing;
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
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void ReadyPossessObject()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isPosessing)
        {
            transform.position = otherObject.transform.position;
            GetComponent<SpriteRenderer>().enabled = false;
            isPosessing = true;
        }

        if (isPosessing)
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
            isPosessing = false;
        }
    }
}
