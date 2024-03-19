using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Posessed : MonoBehaviour
{
    public bool isPosessed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPosessed)
        {
            moving();
        }
        if (isPosessed)
        { 
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    void moving()
    {
        float movingX = Input.GetAxis("Horizontal");
        float movingY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(movingX * speed, 0);
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
