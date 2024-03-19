using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PossesingObject : MonoBehaviour
{
    private GameObject otherObject;

    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        //DialogueManager.instance.endDialogue += Instance_endDialogue;
        //DialogueManager.instance.beginDialogue += Instance_beginDialogue;
    }

    private void Instance_beginDialogue()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
    }

    private void Instance_endDialogue()
    {
        PlayerControllerManager.instance.InvokeInterract -= Instance_InvokeInterract;
    }

    private void Instance_InvokeInterract()
    {
        if (!Ghost.isPosessing && otherObject != null)
        {
            Debug.Log("Posessing Object");
            Ghost.isPosessingObject = true;
            otherObject.GetComponent<Objects>().isPosessed = true;
            return;
        }

        if (Ghost.isPosessingObject)
        {
            Debug.Log("Keluar dari Object");
            Ghost.isPosessingObject = false;
            otherObject.GetComponent<Objects>().isPosessed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if(otherObject != null && otherObject.GetComponent<Objects>().canPosessed && Ghost.isPosessing)
        {
            ReadyPossessObject();
        }*/
        if (Ghost.isPosessingObject)
        {
            transform.position = otherObject.transform.position;
        }
    }

    /*void ReadyPossessObject()
    {
        //Debug.Log("Ready Posess Object");
        if (Ghost.isPosessingObject)
        {
            otherObject.GetComponent<Objects>().possess();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (otherObject == null && collision.tag == "object" && collision.GetComponent<Objects>().canPosessed)
        {
            otherObject = collision.gameObject;
            otherObject.GetComponent<SpriteRenderer>().color = Color.black;
            //Debug.Log("Siap Merasuki " + gameObject);
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
