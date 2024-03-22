using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PosessPerson : MonoBehaviour
{
    public GameObject targetPosess;

    public void Instance_InvokeInterract(Ghost ghost, GameObject gameObject)
    {
        if(ghost != null)
        {
            targetPosess = gameObject;
        }
        else
        {
            targetPosess = null;
        }
        if (ghost.IsUltimateForm && gameObject.TryGetComponent<GhostBuster>(out GhostBuster buster))
        {
            UltimateAction(ghost, gameObject);
            return;
        }
        else if(gameObject.TryGetComponent<NPC>(out NPC npc))
        {
            if(npc.fearMeter >= 3 && npc.type == NPC.NPC_Type.Child)
            {
                npc.GetMoveAction().StopAllCoroutines();
                npc.isPosessed = true;
                npc.GetAnimator().SetFloat("IsMoving", -1);
                npc.GetAnimator().GetComponent<SpriteRenderer>().color = new Color(210, 130, 130);
                Posessed posessed = targetPosess.GetComponent<Posessed>();
                posessed.isPosessed = true;
                
                ghost.SetInvisibility(true);
                ghost.npcPosessed = npc;
                Ghost.isPosessingPerson = true;
            }
        }
    }
    private void UltimateAction(Ghost ghost, GameObject gameObject)
    {
        
        if(gameObject.TryGetComponent<GhostBuster>(out GhostBuster buster))
        {
            if (buster.CheckVunerability())
            {
                buster.Fear();
                ghost.UseUltimateEffect();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Ghost.isPosessingPerson)
        {
            transform.position = targetPosess.transform.position;
        }
    }
    
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(targetPosess == null && collision.tag== "PeopleInside") 
        {
            //Debug.Log("Ready Poesess Person");
            targetPosess = collision.gameObject.transform.parent.gameObject;
            Debug.Log("Ready to posess " + targetPosess.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Ghost.isPosessing && targetPosess != null && collision.tag == "PeopleInside")
        {
            targetPosess = null;
        }
    }*/
}
