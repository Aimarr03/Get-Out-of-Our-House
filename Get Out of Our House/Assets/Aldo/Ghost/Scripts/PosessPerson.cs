using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PosessPerson : MonoBehaviour
{
    public GameObject targetPosess;

    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        //DialogueManager.instance.beginDialogue += Instance_beginDialogue;
        //DialogueManager.instance.endDialogue += Instance_endDialogue;
    }

    private void Instance_endDialogue()
    {
        PlayerControllerManager.instance.InvokeInterract -= Instance_InvokeInterract;
    }

    private void Instance_beginDialogue()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
    }

    private void Instance_InvokeInterract()
    {
        if (targetPosess != null && targetPosess.GetComponent<FearMeter>().fearMeter >= 5)
        {
            Debug.Log("Poesess Person");
            targetPosess.GetComponent<SpriteRenderer>().color = Color.blue;
            GetComponent<SpriteRenderer>().enabled = false;
            targetPosess.GetComponent<Posessed>().isPosessed = true;
            Ghost.isPosessingPerson = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPosess != null &&  Input.GetKeyDown(KeyCode.F) && targetPosess.GetComponent<FearMeter>().fearMeter >= 5) 
        {
            Debug.Log("Poesess Person");
            targetPosess.GetComponent<Posessed>().isPosessed = true; 
            Ghost.isPosessingPerson = true;
        }
        if (Ghost.isPosessingPerson)
        {
            transform.position = targetPosess.transform.position;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(targetPosess == null && collision.tag=="People") 
        {
            Debug.Log("Ready Poesess Person");
            targetPosess = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targetPosess != null && collision.tag == "People")
        {
            targetPosess = null;
        }
    }
}
