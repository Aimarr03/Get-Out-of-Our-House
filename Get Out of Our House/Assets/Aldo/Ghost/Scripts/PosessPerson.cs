using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PosessPerson : MonoBehaviour
{
    public GameObject targetPosess;
    private Ghost ghost;
    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }

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
        if (ghost.IsUltimateForm)
        {
            UltimateAction();
            return;
        }
        if (targetPosess != null && targetPosess.GetComponent<FearMeter>().fearMeter >= 5)
        {
            Debug.Log("Poesess Person");
            targetPosess.GetComponent<Posessed>().isPosessed = true;
            Ghost.isPosessingPerson = true;
        }
    }
    private void UltimateAction()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 6, ghost.ghostBusterLayerMask);
        Debug.Log(collision.ToString());
        if(collision.TryGetComponent<GhostBuster>(out GhostBuster buster))
        {
            if (buster.CheckVunerability())
            {
                buster.Fear();
                ghost.UseUltimateEffect();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Ghost.isPosessingPerson)
        {
            transform.position = targetPosess.transform.position;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(targetPosess == null && collision.tag== "PeopleInside") 
        {
            //Debug.Log("Ready Poesess Person");
            targetPosess = collision.gameObject.transform.parent.gameObject;
            Debug.Log("Ready to posess " + targetPosess.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Ghost.isPosessing && targetPosess != null && collision.tag == "PeopleInside")
        {
            targetPosess = null;
        }
    }
}
