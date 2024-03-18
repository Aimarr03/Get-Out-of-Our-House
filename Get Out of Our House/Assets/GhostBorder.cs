using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBorder : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().enabled = parent.GetComponentInParent<SpriteRenderer>().enabled;
    }
}
