using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 17), Time.deltaTime);
        if (transform.position.y == 17)
        {
            SceneLoader.LoadScene(0);
            return;
        }
    }
}
