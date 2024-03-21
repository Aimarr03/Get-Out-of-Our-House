using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform targetCenter;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        Ghost ghost = targetCenter.GetComponent<Ghost>();
        Room room = ghost.GetCurrentRoom();
        //ghost.transform.position = transform.position;
        transform.position = new Vector3(targetCenter.position.x, room.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log("transform " + transform.position);
        Ghost ghost = targetCenter.GetComponent<Ghost>();
        Room room = ghost.GetCurrentRoom();
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(targetCenter.position);
        room.GetGroundHorizontalBoundForCamera(out float min, out float max);
        Vector3 maxPortPosition = Camera.main.WorldToViewportPoint(new Vector3(max,0,0));
        Vector3 minPortPosition = Camera.main.WorldToViewportPoint(new Vector3(min, 0, 0));
        //Debug.Log(minPortPosition);
        float moveSpeed = 0.1f;

        // Adjust camera position based on viewport positions
        if (viewportPoint.x < 0.3f && minPortPosition.x < 0)
        {
            // Move camera left
            transform.position -= new Vector3(moveSpeed, 0, 0);
        }
        else if (viewportPoint.x > 0.8f && maxPortPosition.x > 1)
        {
            // Move camera right
            transform.position += new Vector3(moveSpeed, 0, 0);
        }
    }
    
}
