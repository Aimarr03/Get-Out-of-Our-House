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
    [SerializeField] private bool readyOpen = false;
    [SerializeField] public static bool AtFrontDoor = false;
    [SerializeField] private AudioSource openDoorSound;
    private Color color;

    public static void OpenDoorMenu()
    {
        AtFrontDoor = true;
    }

    private void Awake()
    {
        color = bg.GetComponent<SpriteRenderer>().color;
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
    }

    private void Instance_InvokeInterract()
    {
        if (readyOpen)
        {
            AtFrontDoor = true;
        }
    }

    private void LateUpdate()
    {
        Debug.Log("Updated");
        Debug.Log("Current Target " + CurrentTarget);
        if (Camera.main.orthographicSize <= 1) 
        {
            SceneLoader.LoadScene(1);
            return;
        }
        if (CurrentTarget != null)
        {
            MainCamera.position = Vector3.MoveTowards(MainCamera.position, new Vector3(CurrentTarget.position.x, CurrentTarget.position.y, MainCamera.position.z), speed * Time.deltaTime);
            if(new Vector2(MainCamera.position.x, MainCamera.position.y) == new Vector2(CurrentTarget.position.x, CurrentTarget.position.y))
            {
                StartCoroutine(changeTarget());
                speed = 3;
            }
        }

        if (CurrentTarget == Target2 && Camera.main.orthographicSize > 3)
        {
            if (!readyOpen)
            {
                readyOpen = true;
            }
            Camera.main.orthographicSize -= 1 * Time.deltaTime;
        }
        
        if (AtFrontDoor)
        {
            Debug.Log("AtFrontDoor " + AtFrontDoor);
            if (!openDoorSound.isPlaying)
            {
                openDoorSound.Play();
            }
            KnockingMenu.playKnockingMenu = true;
            color.a -= Time.deltaTime * 0.5f;
            bg.GetComponent<SpriteRenderer>().color = color;
            speed = 7;
            if (Camera.main.orthographicSize > 1) Camera.main.orthographicSize -= 1 * Time.deltaTime;
        }
    }

    public void StartCamera()
    { 
        CurrentTarget = Target1;
        Debug.Log("Targettttt");
    }

    IEnumerator changeTarget()
    {
        PlayerControllerManager.instance.InvokeInterract += Instance_InvokeInterract;
        yield return new WaitForSeconds(0.5f);
        CurrentTarget = Target2;
    }
}
