using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Move_Action : MonoBehaviour
{
    #region testing
    [SerializeField] private Transform TargetLocation;
    #endregion
    [SerializeField] private Vector3 targetLocation;
    [SerializeField] private float movementSpeed;
    private bool canMove;
    private void Start()
    {
        SetTargetLocation(TargetLocation.position);
        canMove = true;
    }
    private void Update()
    {
        if (!canMove) return;
        if (Vector3.Distance(targetLocation, transform.position) < 0.1f)
        {
            canMove = false;
        }
        MovingTowards();
    }
    public void SetTargetLocation(Vector3 targetLocation)
    {
        canMove = true;
        targetLocation.y = transform.position.y;
        this.targetLocation = targetLocation;
    }
    public void MovingTowards()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = transform.position.y;
        transform.position = Vector3.MoveTowards(currentPosition, targetLocation, movementSpeed * Time.deltaTime);
    }
}
