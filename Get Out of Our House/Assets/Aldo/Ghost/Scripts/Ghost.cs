using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public static bool isPosessingObject;
    public static bool isPosessingPerson;
    public static bool isPosessing;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moving();
        if (isPosessingObject || isPosessingPerson)
        {
            isPosessing = true;
        }
        else
        {
            isPosessing = false;
        }
        if(isPosessingPerson)
        {
            transform.position = GetComponent<PosessPerson>().targetPosess.transform.position;
        }
    }

    void moving()
    {
        float movingX = Input.GetAxis("Horizontal");
        float movingY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(movingX * speed, movingY * speed);
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 6;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 3;
        }
    }
}
