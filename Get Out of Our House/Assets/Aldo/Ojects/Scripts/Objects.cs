using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public enum ObjectType
    {
        Lightable,
        Fallable
    }
    public bool isPosessed = false;
    public bool canPosessed = true;
    public bool canShake = true;
    public bool canDrop = false;
    private GameObject targetPeople;
    private SpriteRenderer interractedVisual;

    [SerializeField] private float radiusCollision;
    [SerializeField] private Sprite TouchFloorSprite;
    [SerializeField] private ObjectType objectType;
    [SerializeField] private Transform lightSource;
    [SerializeField] private bool currentInterract = true;
    // Start is called before the first frame update
    void Start()
    {
        interractedVisual = GetComponent<SpriteRenderer>();
        InterractEffect(currentInterract);
        PlayerControllerManager.instance.InvokeAction1 += Instance_InvokeAction1;
        PlayerControllerManager.instance.InvokeAction2 += Instance_InvokeAction2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Instance_InvokeAction1()
    {
        if (!canShake || !isPosessed) return;
        switch (objectType)
        {
            case ObjectType.Fallable:
                break;
            case ObjectType.Lightable:
                canShake = false;
                canPosessed = false;
                StartCoroutine(Shaking());
                AttemptFearGhostBuster();
                break;
        }
        
    }
    private void AttemptFearGhostBuster()
    {
        Debug.Log(LayerMask.NameToLayer("Ghost Buster"));
        int layer = LayerMask.NameToLayer("Ghost Buster");
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(8, 8), 0);
        Collider2D colliderGhostBuster = null;
        foreach (Collider2D collision in collisions)
        {
            Debug.Log("Ghost Buster detected = "+ collision);
            if (collision.TryGetComponent<GhostBuster>(out GhostBuster ghostBuster))
            {
                colliderGhostBuster = collision;
                break;
            }
        }
        if(colliderGhostBuster != null)
        {
            Transform ghostPosition = colliderGhostBuster.transform;
            GhostBuster ghostBuster = colliderGhostBuster.GetComponent<GhostBuster>();
            Vector3 direction = (ghostPosition.position - transform.position);
            float x_direction = direction.x;
            if(x_direction > 0  && ghostBuster.currentDirection == GhostBuster.StateDirection.Right)
            {
                ghostBuster.Surprised();
            }
            else if (x_direction < 0 && ghostBuster.currentDirection == GhostBuster.StateDirection.Left)
            {
                ghostBuster.Surprised();
            }
        }
    }
    private void Instance_InvokeAction2()
    {
        if (!canDrop || !isPosessed) return;
        switch (objectType)
        {
            case ObjectType.Fallable:
                if (!canDrop) break;
                canDrop = false;
                GetComponent<Rigidbody2D>().gravityScale = 10;
                GetComponent<Objects>().canPosessed = false;
                StartCoroutine(Fading());
                break;
            case ObjectType.Lightable:
                break;
        }
        Ghost.isPosessing = false;
    }

    /*public void possess()
    {
        
    }*/

    public IEnumerator Fading()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public IEnumerator Shaking()
    {
        // Yang dilakukan ketika Action 1 (animasi)
        currentInterract = !currentInterract;
        InterractEffect(currentInterract);
        //GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        if (targetPeople != null)
        {
            targetPeople.GetComponent<FearMeter>().fearMeter += 5;
        }
        //GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //GetComponent<SpriteRenderer>().color = Color.white;
        canShake = true;
        canPosessed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "People")
        {
            targetPeople = collision.gameObject;
        }
        if(collision.tag == "Floor")
        {
            Debug.Log("Menyentuh Floor");
            interractedVisual.sprite = TouchFloorSprite;
            Collider2D ghostBusterCollider = Physics2D.OverlapBox(transform.position, new Vector2(8,8), 0, LayerMask.NameToLayer("Ghost Buster"));
            Debug.Log("Ghost Buster detected = " + (ghostBusterCollider != null));
            if (ghostBusterCollider != null)
            {
                GhostBuster ghostBuster = ghostBusterCollider.GetComponent<GhostBuster>();
                if (ghostBuster.CheckVunerability())
                {
                    ghostBuster.Fear();
                }
                else
                {
                    ghostBuster.Surprised();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "People")
        {
            targetPeople = null;
        }
    }
    private void InterractEffect(bool input)
    {
        interractedVisual.enabled = input;
        if (lightSource != null)
        {
            lightSource.gameObject.SetActive(input);
        }   
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusCollision);
    }
}
