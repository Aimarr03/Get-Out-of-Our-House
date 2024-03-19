using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform maxLeft;
    public Transform maxRight;
    public Transform targetCenter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = targetCenter.position.x;
        if(xPos > maxRight.position.x ) xPos = maxRight.position.x;
        if(xPos < maxLeft.position.x ) xPos = maxLeft.position.x;
        Vector3 targetPos = new Vector3(xPos, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, (Ghost.speed * 8 / 10) * Time.deltaTime);
    }   
}
