using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        
        // Can be passed through to underlying traversement system
        Debug.Log(context.ReadValue<Vector2>().ToString());
    }
}
