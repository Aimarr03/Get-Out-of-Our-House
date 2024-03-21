using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuAnimation : MonoBehaviour
{
    [SerializeField] private Transform MainCamera;
    [SerializeField] private GameObject bg;
    [SerializeField] private static Transform CurrentTarget;
    [SerializeField] private Transform Target1;
    [SerializeField] private Transform Target2;
    [SerializeField] private float speed = 10;
    [SerializeField] public static bool AtFrontDoor = false;
    private Color color;

    public static void SetCurrentTargetCamera(Transform TargetCamera)
    {
        CurrentTarget = TargetCamera;
    }

    public static void OpenDoorMenu()
    {
        AtFrontDoor = true;
    }

    private void Start()
    {
        color = bg.GetComponent<SpriteRenderer>().color;
    }

    private void FixedUpdate()
    {
        if (CurrentTarget != null)
        {
            MainCamera.position = Vector3.MoveTowards(MainCamera.position, new Vector3(CurrentTarget.position.x, CurrentTarget.position.y, MainCamera.position.z), speed * Time.deltaTime);
            if(new Vector2(MainCamera.position.x, MainCamera.position.y) == new Vector2(CurrentTarget.position.x, CurrentTarget.position.y))
            {
                StartCoroutine(changeTarget());
                speed = 3;
            }
        }

        if (CurrentTarget == Target2 && Camera.main.orthographicSize > 3) Camera.main.orthographicSize -= 1 * Time.deltaTime;
        
        if (AtFrontDoor)
        {
            color.a -= Time.deltaTime * 0.5f;
            bg.GetComponent<SpriteRenderer>().color = color;
            speed = 7;
            if (Camera.main.orthographicSize > 1) Camera.main.orthographicSize -= 1 * Time.deltaTime;
        }
    }

    public void StartCamera()
    {
        CurrentTarget = Target1;
    }

    IEnumerator changeTarget()
    {
        yield return new WaitForSeconds(0.5f);
        CurrentTarget = Target2;
    }
}
