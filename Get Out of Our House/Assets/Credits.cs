using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private void Start()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
    }

    private void Instance_InvokeInterract()
    {
        SceneLoader.LoadScene(0);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 17), Time.deltaTime);    
    }
}
