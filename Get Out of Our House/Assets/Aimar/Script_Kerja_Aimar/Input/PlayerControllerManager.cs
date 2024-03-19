using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    public static PlayerControllerManager instance;
    public event Action InvokeInterract;
    public event Action InvokeAltInterract;
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
        newPlayerController.PlayerController.Alt_Interract.performed += Alt_Interract_performed;
    }

    private void Alt_Interract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Invoke Alt Interract Manager");
        InvokeAltInterract?.Invoke();
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
