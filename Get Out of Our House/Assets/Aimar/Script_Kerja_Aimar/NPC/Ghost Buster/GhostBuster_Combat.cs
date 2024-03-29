using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuster_Combat : MonoBehaviour
{
    [SerializeField] private Vector3 aggroSize;
    [SerializeField] private Vector3 attackSize;
    [SerializeField] private float pullForce;
    [SerializeField] private LayerMask ghostLayerMask;
    private int damage;
    private float currentAttackInterval;
    private float attackInterval;
    private float preparingAttackDuration;
    private float currentPrepareDuration;

    private GhostBuster ghostBuster;
    public enum CombatState
    {
        Non,
        Aggro,
        Attack,
        Feared,
    }
    public CombatState currentState;
    private void Awake()
    {
        ghostBuster = GetComponent<GhostBuster>();
        currentState = CombatState.Non;
        damage = 3;
        currentAttackInterval = 0;
        currentPrepareDuration = 0;
        attackInterval = 5f;
        preparingAttackDuration = 0.4f;
    }
    private void Start()
    {
        ghostBuster.GhostDetected += GhostBuster_GhostDetected;
    }


    private void GhostBuster_GhostDetected(bool detected, Room room)
    {
        if (detected)
        {
            currentState = CombatState.Aggro;
        }
        else
        {
            currentState = CombatState.Non;
        }
    }

    private void Update()
    {
        if (ghostBuster.Insane) return;
        switch (currentState)
        {
            case CombatState.Non:
                break;
            case CombatState.Aggro:
                AggroState();
                break;
            case CombatState.Attack:
                AggroState();
                AttackingState();
                break;
            case CombatState.Feared:
                FearState();
                break;
        }
    }
    private void AggroState()
    {
        //Ghost ghost = ghostBuster.IsGhostDetected();
        SoundManager.instance.sucking.Stop();
        GhostBuster_Move_Action moveAction = ghostBuster.GetMoveAction();
        
        Vector3 offsetPosition = new Vector3(transform.position.x, transform.position.y + aggroSize.y / 2, transform.position.z);
        Collider2D ghostCollided = Physics2D.OverlapBox(offsetPosition, aggroSize, 0, ghostLayerMask);
        if (ghostCollided != null)
        {
            if (currentState == CombatState.Aggro)
            {
                currentPrepareDuration = 0;
                currentAttackInterval = 0;
            }
            //Debug.Log("Ghost Detected");
            currentState = CombatState.Attack;
            ghostBuster.GetAnimator().SetFloat("IsMoving", -1);
        }
        else
        {
            Ghost ghost = ghostBuster.IsGhostDetected();
            if (ghost == null) return;
            Vector3 ghostPosition = ghost.transform.position;
            ghostPosition = new Vector3(ghostPosition.x, transform.position.y, ghostPosition.z);
            ghostBuster.FlippingSprite(ghostPosition);
            ghostBuster.GetAnimator().SetFloat("IsMoving", 1);
            //Debug.Log("Ghost Approaching");
            transform.position = Vector2.MoveTowards(transform.position, ghostPosition, moveAction.GetMovementSpeed() * Time.deltaTime);
            currentState = CombatState.Aggro;
        }
    }
    private void AttackingState()
    {
        if (currentPrepareDuration < preparingAttackDuration)
        {
            //Debug.Log("Preparing To Attack");
            currentPrepareDuration += Time.deltaTime;
            return;
        }
        if (currentState == CombatState.Aggro) return;
        Vector3 offsetPosition = new Vector3(transform.position.x, transform.position.y + aggroSize.y / 2, transform.position.z);
        Collider2D ghostCollided = Physics2D.OverlapBox(offsetPosition, aggroSize, 0, ghostLayerMask);
        if (ghostCollided == null) return;
        Rigidbody2D rigidBody = ghostCollided.GetComponent<Rigidbody2D>();
        ghostBuster.FlippingSprite(ghostCollided.transform.position);
        PullAttack(rigidBody);
    }
    private void PullAttack(Rigidbody2D ghostRigidbody)
    {
        if (ghostRigidbody.GetComponent<Ghost>().IsUltimateForm) return;
        Vector2 direction = (ghostBuster.CenterPosition.position - ghostRigidbody.transform.position).normalized;
        ghostRigidbody.AddForce(direction * pullForce);
        //Debug.Log("Pulling Ghost");
        currentAttackInterval += Time.deltaTime;
        if (currentAttackInterval >= attackInterval)
        {
            if (!SoundManager.instance.sucking.isPlaying) SoundManager.instance.sucking.Play();
            currentAttackInterval = 0;
            ghostRigidbody.GetComponent<Ghost>().Damage();
            ghostRigidbody.GetComponent<Ghost>().getAnimator().SetTrigger("Damage");
            Debug.Log("Ghost Take Damage " + damage);
        }
    }
    private void FearState()
    {

    }
    private void OnDrawGizmos()
    {
        /*Vector3 offsetPosition = new Vector3(transform.position.x, transform.position.y + aggroSize.y / 2, transform.position.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(offsetPosition, aggroSize);
        offsetPosition = new Vector3(transform.position.x, transform.position.y + attackSize.y / 2, transform.position.z);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(offsetPosition, attackSize);*/
    }
}
