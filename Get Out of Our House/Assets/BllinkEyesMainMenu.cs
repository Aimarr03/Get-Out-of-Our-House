using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BllinkEyesMainMenu : MonoBehaviour
{
    private GameObject[] eyes = new GameObject[2];
    private float openEyes;
    private bool isOpen = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            eyes[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            StartCoroutine(blink());
            isOpen = false;
        }
    }

    IEnumerator blink()
    {
        yield return new WaitForSeconds(5f);
        foreach (var item in eyes)
        {
            item.SetActive(false);   
        }
        yield return new WaitForSeconds(3f);
        foreach (var item in eyes)
        {
            item.SetActive(true);
        }
        isOpen = true;
    }
}
