using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabOther : MonoBehaviour
{
    private GameObject otherPeople;
    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
    }

    private void Instance_InvokeInterract()
    {
        Debug.Log("Menusuk Orang");
        if (Ghost.isPosessingPerson && otherPeople != null)
        {
            Destroy(otherPeople);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(otherPeople == null && collision.tag == "PeopleInside")
        {
            otherPeople = collision.gameObject.transform.parent.gameObject;
            Debug.Log("Ketemu orang Lain " + otherPeople.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (otherPeople == null && collision.tag == "PeopleInside")
        {
            Debug.Log("Menjauhi orang Lain");
            otherPeople = null;
        }
    }
}
