using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockingMenu : MonoBehaviour
{
    public static bool playKnockingMenu;

    // Update is called once per frame
    void Update()
    {
        if (playKnockingMenu) GetComponent<Animator>().Play("knock");
    }
}
