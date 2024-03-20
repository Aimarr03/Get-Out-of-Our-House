using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    public static PlayerControllerManager instance;
    public event Action InvokeInterract;
    public event Action InvokeAction1;
    public event Action InvokeAction2;
    public event Action InvokeUltimate;
    private NewPlayerController newPlayerController;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        newPlayerController = new NewPlayerController();
        instance = this;
    }
    private void Start()
    {
        newPlayerController.PlayerController.Enable();
        newPlayerController.PlayerController.Interract.performed += Interract_performed;
        newPlayerController.PlayerController.Action1.performed += Action1_performed;
        newPlayerController.PlayerController.Action2.performed += Action2_performed;
        newPlayerController.PlayerController.Ultimate.performed += Ultimate_performed;
    }

    private void Ultimate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Ultimate Interract Manager");
        InvokeUltimate?.Invoke();
    }

    private void Action1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Invoke Act1 Interract Manager");
        InvokeAction1?.Invoke();
    }
    private void Action2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Invoke Act2 Interract Manager");
        InvokeAction2?.Invoke();
    }

    private void Interract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Invoke Interract Manager");
        InvokeInterract?.Invoke();
    }

    public Vector2 GetVector2Input()
    {
        return newPlayerController.PlayerController.Move.ReadValue<Vector2>();
    }
    
}
