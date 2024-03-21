using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PossesingObject : MonoBehaviour
{
    private GameObject otherObject;
    void Start()
    {
        
    }
    public void Instance_InvokeInterract(Ghost ghost, GameObject gameObject)
    {
        if (ghost != null)
        {
            otherObject = gameObject;
        }
        else
        {
            otherObject = null;
        }
        if (ghost.IsUltimateForm) return;
        if (!Ghost.isPosessing && otherObject != null)
        {
            Debug.Log("Posessing Object");
            ghost.SetInvisibility(true);
            otherObject.GetComponent<Objects>().isPosessed = true;
            otherObject.GetComponent<Objects>().SetGhost(ghost);
            return;
        }

        if (Ghost.isPosessingObject)
        {
            Debug.Log("Keluar dari Object");
            ghost.SetInvisibility(false);
            otherObject.GetComponent<Objects>().isPosessed = false;
            otherObject.GetComponent<Objects>().SetGhost(null);
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

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (otherObject == null && collision.tag == "object" && collision.GetComponent<Objects>().canPosessed)
        {
            otherObject = collision.gameObject;
            //otherObject.GetComponent<SpriteRenderer>().color = Color.black;
            //Debug.Log("Siap Merasuki " + gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (otherObject != null && collision.tag == "object")
        {
            //otherObject.GetComponent<SpriteRenderer>().color = Color.white;
            otherObject = null;
            if (!Ghost.isPosessingPerson)
            {
                Ghost.isPosessingObject = false;
            }
        }
    }*/
}
